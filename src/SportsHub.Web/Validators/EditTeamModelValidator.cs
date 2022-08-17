using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;

namespace SportsHub.Web.Validators
{
    internal class EditTeamModelValidator : AbstractValidator<EditTeamModel>
    {
        private readonly ISubcategoryService _subcategoryService;
        private readonly ITeamService _teamService;

        public EditTeamModelValidator(
            ISubcategoryService subcategoryService,
            ITeamService teamService)
        {
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));

            RuleFor(team => team.Name)
                .MustAsync((name, cancellation) => DoesTeamNameIsUniqueAsync(name)).WithMessage("Team with that name already exists!");

            RuleFor(team => team.SubcategoryId)
                .MustAsync((id, cancellation) => _subcategoryService.DoesSubcategoryAlreadyExistByIdAsync(id))
                .WithMessage("Subcategory with that id does not exist!");
            
        }

        private async Task<bool> DoesTeamNameIsUniqueAsync(string teamName)
        {
            var result = await _teamService.DoesTeamAlreadyExistByNameAsync(teamName);

            return !result;
        }
    }
}
