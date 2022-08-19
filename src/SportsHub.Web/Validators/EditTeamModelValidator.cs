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
                .NotEmpty().WithMessage(Errors.TeamIdCannotBeEmpty)
                .MustAsync((id, cancellation) => _teamService.DoesTeamAlreadyExistByIdAsync(id))
                .WithMessage(Errors.TeamIdDoesNotExist);

            RuleFor(team => team.Name)
                .NotEmpty().WithMessage(Errors.TeamNameCannotBeEmpty)
                .MustAsync((name, cancellation) => DoesTeamNameIsUniqueAsync(name))
                .WithMessage(Errors.TeamNameIsNotUnique);


            RuleFor(team => team.SubcategoryId)
                .NotEmpty().WithMessage(Errors.SubcategoryIdCannotBeEmpty)
                .MustAsync((id, cancellation) => _subcategoryService.DoesSubcategoryAlreadyExistByIdAsync(id))
                .WithMessage(Errors.SubcategoryDoesNotExist);

            RuleFor(teamLogo => teamLogo.Logo)
                .NotEmpty().WithMessage(Errors.TeamLogoCannotBeEmpty);

            When(teamLogo => teamLogo.Logo != null, () =>
            {
                RuleFor(teamLogo => teamLogo.Logo)
                    .MustAsync((teamLogo, cancellation) => DoesTeamLogoHaveSatisfactoryExtension(teamLogo))
                    .WithMessage(Errors.TeamLogoCannotHaveThisExtension);
            });
        }

        private async Task<bool> DoesTeamNameIsUniqueAsync(string teamName)
        {
            var result = await _teamService.DoesTeamAlreadyExistByNameAsync(teamName);

            return !result;
        }

        private Task<bool> DoesTeamLogoHaveSatisfactoryExtension(IFormFile teamLogo)
        {
            var formFile = teamLogo;
            var fileExtension = "";

            if (formFile != null)
                fileExtension = Path.GetExtension(formFile.FileName);

            if (fileExtension == ".JPG" || fileExtension == ".PNG" || fileExtension == ".SVG" ||
                fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".svg")
                return Task.FromResult(true);
            else
                return Task.FromResult(false);
        }
    }
}
