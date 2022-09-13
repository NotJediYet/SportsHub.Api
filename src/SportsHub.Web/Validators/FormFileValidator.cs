using FluentValidation;
using SportsHub.Shared.Resources;

namespace SportsHub.Web.Validators
{
    public class FormFileValidator : AbstractValidator<IFormFile>
    {
        private const string Extension = @"\.jpg|\.png|\.PNG|\.svg";
        public FormFileValidator()
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
