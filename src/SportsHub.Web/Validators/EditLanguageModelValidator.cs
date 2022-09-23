using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;

namespace SportsHub.Web.Validators
{
    public class EditLanguageModelValidator : AbstractValidator<EditLanguageModel>
    {
        private readonly ILanguageService _languageService;

        public EditLanguageModelValidator(ILanguageService languageService)
        {
            _languageService = languageService ?? throw new ArgumentNullException(nameof(languageService));

            RuleFor(language => language.Id)
                .NotEmpty().WithMessage(Errors.LanguageIdCannotBeEmpty)
                .MustAsync((id, cancellation) => _languageService.DoesLanguageAlreadyExistByIdAsync(id))
                .WithMessage(Errors.LanguageDoesNotExist);

            RuleFor(language => language)
                .MustAsync((language, cancellation) => DoesLanguageNameIsUniqueAsync(language.Name, language.Id))
                .WithMessage(Errors.LanguageNameIsNotUnique);

            RuleFor(language => language.Name)
                .NotEmpty().WithMessage(Errors.LanguageNameCannotBeEmpty);

            RuleFor(language => language)
                .MustAsync((language, cancellation) => DoesLanguageCodeIsUniqueAsync(language.Code, language.Id))
                .WithMessage(Errors.LanguageCodeIsNotUnique);

            RuleFor(language => language.Code)
                .NotEmpty().WithMessage(Errors.LanguageCodeCannotBeEmpty);
        }

        private async Task<bool> DoesLanguageNameIsUniqueAsync(string languageName, Guid Id)
        {
            var result = await _languageService.GetLanguageIdByNameAsync(languageName);

            if (result == Guid.Empty)
            {
                return true;
            }
            else if (result == Id) { return true; }
            else return false;
        }

        private async Task<bool> DoesLanguageCodeIsUniqueAsync(string languageCode, Guid Id)
        {
            var result = await _languageService.GetLanguageIdByCodeAsync(languageCode);

            if (result == Guid.Empty)
            {
                return true;
            }
            else if (result == Id) { return true; }
            else return false;
        }
    }
}
