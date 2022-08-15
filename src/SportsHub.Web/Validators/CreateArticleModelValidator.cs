using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;

namespace SportsHub.Web.Validators
{
    internal class CreateArticleModelValidator : AbstractValidator<CreateArticleModel>
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
               .NotEmpty().WithMessage("Article headline cannot be empty!")
               .MustAsync((headline, cancellation) => DoesArticleNameIsUniqueAsync(headline))
               .WithMessage("Article with that headline already exists!");

            RuleFor(article => article.TeamId)
                .NotEmpty().WithMessage("Team id cannot be empty!")
                .MustAsync((id, cancellation) => _teamService.DoesTeamAlreadyExistByIdAsync(id))
                .WithMessage("Team with that id does not exist!");
        }

        private async Task<bool> DoesArticleNameIsUniqueAsync(string headline)
        {
            var result = await _articleService.DoesArticleAlreadyExistByHeadlineAsync(headline);

            return !result;
        }
    }
}