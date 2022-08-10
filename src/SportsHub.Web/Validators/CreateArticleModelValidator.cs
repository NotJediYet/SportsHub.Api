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

            RuleFor(article => article.ArticleImage)
                .SetValidator(new IFormFileValidator());

            RuleFor(article => article.TeamId)
                .NotEmpty().WithMessage(Errors.TeamIdCannotBeEmpty)
                .MustAsync((id, cancellation) => _teamService.DoesTeamAlreadyExistByIdAsync(id))
                .WithMessage(Errors.TeamIdDoesNotExist);

        }


        public class IFormFileValidator : AbstractValidator<IFormFile>
        {
            private const string Extension = @"\.jpg|\.png|\.svg";
            public IFormFileValidator()
            {
                SetRules();
            }
            private void SetRules()
            {
                RuleFor(file => Path.GetExtension(file.FileName))
                    .Matches(Extension)
                    .WithMessage(Errors.FileMustHaveAppropriateFormat);
            }
        }


        private async Task<bool> DoesArticleNameIsUniqueAsync(string headline)
        {
            var result = await _articleService.DoesArticleAlreadyExistByHeadlineAsync(headline);

            return !result;
        }
    }
}