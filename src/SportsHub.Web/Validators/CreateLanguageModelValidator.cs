using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;

namespace SportsHub.Web.Validators
{
    public class CreateLanguageModelValidator : AbstractValidator<CreateLanguageModel>
    {
        private readonly ILanguageService _languageService;

        public CreateLanguageModelValidator(ILanguageService languageService)
        {
            _languageService = languageService ?? throw new ArgumentNullException(nameof(languageService));

            RuleFor(language => language.Name)
                .NotEmpty().WithMessage(Errors.LanguageNameCannotBeEmpty)
                .MustAsync((name, cancellation) => DoesLanguageNameIsUniqueAsync(name))
                .WithMessage(Errors.LanguageNameIsNotUnique);


            RuleFor(language => language.Code)
                .NotEmpty().WithMessage(Errors.LanguageCodeCannotBeEmpty)
                .MustAsync((code, cancellation) => DoesLanguageCodeIsUniqueAsync(code))
                .WithMessage(Errors.LanguageCodeIsNotUnique);
        }

        private async Task<bool> DoesLanguageNameIsUniqueAsync(string languageName)
        {
            var result = await _languageService.DoesLanguageAlreadyExistByNameAsync(languageName);

            return !result;
        }

        private async Task<bool> DoesLanguageCodeIsUniqueAsync(string languageCode)
        {
            var result = await _languageService.DoesLanguageAlreadyExistByCodeAsync(languageCode);

            return !result;
        }
    }
}
