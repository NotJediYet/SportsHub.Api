using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;

namespace SportsHub.Web.Validators
{
    public class CreateTeamModelValidator : AbstractValidator<CreateTeamModel>
    {
        private readonly ISubcategoryService _subcategoryService;
        private readonly ITeamService _teamService;

        public CreateTeamModelValidator(
            ISubcategoryService subcategoryService,
            ITeamService teamService)
        {
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));

            RuleFor(team => team.Name)
                .NotEmpty().WithMessage(Errors.TeamNameCannotBeEmpty)
                .MustAsync((name, cancellation) => DoesTeamNameIsUniqueAsync(name))
                .WithMessage(Errors.TeamNameIsNotUnique);

            RuleFor(team => team.SubcategoryId)
                .NotEmpty().WithMessage(Errors.SubcategoryIdCannotBeEmpty)
                .MustAsync((id, cancellation) => _subcategoryService.DoesSubcategoryAlreadyExistByIdAsync(id))
                .WithMessage(Errors.SubcategoryDoesNotExist);

            RuleFor(team => team.TeamLogo)
                .SetValidator(new FormFileValidator());
        }

        private async Task<bool> DoesTeamNameIsUniqueAsync(string teamName)
        {
            var result = await _teamService.GetTeamIdByNameAsync(teamName);

            if (result == Guid.Empty)
            {
                return true;
            }
            else return false;
        }
    }
}
