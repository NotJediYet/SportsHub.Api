using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;
using SportsHub.Shared.Entities;

namespace SportsHub.Web.Validators
{
    public class EditTeamModelValidator : AbstractValidator<Team>
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
                .NotEmpty().WithMessage(Errors.TeamLogoIsRequired)
                .SetValidator(new IFormFileValidator());
        }

        private async Task<bool> DoesTeamNameIsUniqueAsync(string teamName, Guid Id)
        {
            var result = await _teamService.DoesTeamAlreadyExistByNameAsync(teamName);

            if (result == Guid.Empty)
            {
                return true;
            }
            else if (result == Id) { return true; }
            else return false;
            
        }

        public class IFormFileValidator : AbstractValidator<IFormFile>
        {
            private const string Extension = @"\.jpg|\.png|\.PNG|\.svg";
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
