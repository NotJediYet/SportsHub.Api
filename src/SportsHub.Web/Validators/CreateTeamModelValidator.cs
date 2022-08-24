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

            RuleFor(team => team.Logo)
                .NotEmpty().WithMessage(Errors.TeamLogoCannotBeEmpty)
                .SetValidator(new IFormFileValidator());
        }

        private async Task<bool> DoesTeamNameIsUniqueAsync(string teamName)
        {
            var result = await _teamService.DoesTeamAlreadyExistByNameAsync(teamName);

            return !result;
        }
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
}
