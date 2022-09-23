using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;

namespace SportsHub.Web.Validators
{
    public class EditTeamModelValidator : AbstractValidator<EditTeamModel>
    {
        private readonly ISubcategoryService _subcategoryService;
        private readonly ITeamService _teamService;

        public EditTeamModelValidator(
            ISubcategoryService subcategoryService,
            ITeamService teamService)
        {
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));

            RuleFor(team => team.Id)
                .NotEmpty().WithMessage(Errors.TeamIdCanNotBeEmpty)
                .MustAsync((id, cancellation) => _teamService.DoesTeamAlreadyExistByIdAsync(id))
                .WithMessage(Errors.TeamIdDoesNotExist);

            RuleFor(team => team)
                .MustAsync((team, cancellation) => DoesTeamNameIsUniqueAsync(team.Name, team.Id))
                .WithMessage(Errors.TeamNameIsNotUnique);

            RuleFor(team => team.Name)
                .NotEmpty().WithMessage(Errors.TeamNameCannotBeEmpty);

            RuleFor(team => team.SubcategoryId)
                .NotEmpty().WithMessage(Errors.SubcategoryIdCannotBeEmpty)
                .MustAsync((id, cancellation) => _subcategoryService.DoesSubcategoryAlreadyExistByIdAsync(id))
                .WithMessage(Errors.SubcategoryDoesNotExist);
            
            RuleFor(team => team.TeamLogo)
                .SetValidator(new FormFileValidator());
        }

        private async Task<bool> DoesTeamNameIsUniqueAsync(string teamName, Guid Id)
        {
            var result = await _teamService.GetTeamIdByNameAsync(teamName);

            if (result == Guid.Empty)
            {
                return true;
            }
            else if (result == Id) { return true; }
            else return false;
        }
    }
}
