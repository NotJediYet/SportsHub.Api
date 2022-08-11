using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;

namespace SportsHub.Web.Validators
{
    internal class CreateLogoModelValidator : AbstractValidator<CreateLogoModel>
    {
        private readonly ITeamService _teamService;
        private readonly ILogoService _logoService;

        public CreateLogoModelValidator(
            ITeamService teamService,
            ILogoService logoService)
        {
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
            _logoService = logoService ?? throw new ArgumentNullException(nameof(logoService));

            RuleFor(logo => logo.Bytes)
                .NotEmpty().WithMessage("Logo data can not be empty!")
                .MustAsync((bytes, cancellation) => DoesLogoDataIsUniqueAsync(bytes)).WithMessage("This logo already exists!");

            RuleFor(logo => logo.TeamId)
                .NotEmpty().WithMessage("Team id can not be empty!")
                .MustAsync((id, cancellation) => _teamService.DoesTeamAlreadyExistByIdAsync(id))
                .WithMessage("Subcategory with that id does not exist!");
        }

        private async Task<bool> DoesLogoDataIsUniqueAsync(byte[] bytes)
        {
            var result = await _logoService.DoesLogoAlreadyExistByBytesAsync(bytes);

            return !result;
        }
    }
}
