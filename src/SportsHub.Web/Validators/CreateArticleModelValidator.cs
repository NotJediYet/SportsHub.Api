using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;

namespace SportsHub.Web.Validators
{
    public class CreateArticleModelValidator : AbstractValidator<CreateArticleModel>
    {
        private readonly ITeamService _teamService;
        private readonly IArticleService _articleService;

        public CreateArticleModelValidator(
            ITeamService teamService,
            IArticleService articleService)
        {
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
            _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));

            RuleFor(article => article.Headline)
               .NotEmpty().WithMessage(Errors.ArticleHeadlineCannotBeEmpty)
               .MustAsync((headline, cancellation) => DoesArticleNameIsUniqueAsync(headline))
               .WithMessage(Errors.ArticleHeadlineIsNotUnique);

            RuleFor(article => article.TeamId)
                .NotEmpty().WithMessage(Errors.TeamIdCannotBeEmpty)
                .MustAsync((id, cancellation) => _teamService.DoesTeamAlreadyExistByIdAsync(id))
                .WithMessage(Errors.TeamIdDoesNotExist);

            When(article => article.ArticleImage != null, () =>
            {
             RuleFor(article => article.ArticleImage)
                .MustAsync((articleImage, cancellation) => DoesArticleImageHaveCorectExtension(articleImage))
                .WithMessage(Errors.DoesArticleImageCannotHaveThisExtension);
            });
            
        }
        private Task<bool> DoesArticleImageHaveCorectExtension(IFormFile image)
        {
            var fileExtension = ""; 

            if (image != null)
                fileExtension = Path.GetExtension(image.FileName);

            var result = (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".svg") ? Task.FromResult(true):Task.FromResult(false);

            return result;
        }
        private async Task<bool> DoesArticleNameIsUniqueAsync(string headline)
        {
            var result = await _articleService.DoesArticleAlreadyExistByHeadlineAsync(headline);

            return !result;
        }
    }
}