using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;

namespace SportsHub.Web.Validators
{
    public class CreateCategoryModelValidator : AbstractValidator<CreateCategoryModel>
    {
        private readonly ICategoryService _categoryService;

        public CreateCategoryModelValidator(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));

            RuleFor(category => category.Name)
                .NotEmpty().WithMessage(Errors.CategoryNameCannotBeEmpty)
                .MustAsync((name, cancellation) => DoesCategoryNameIsUniqueAsync(name))
                .WithMessage(Errors.CategoryNameIsNotUnique);
        }

        private async Task<bool> DoesCategoryNameIsUniqueAsync(string categoryName)
        {
            var result = await _categoryService.DoesCategoryAlreadyExistByNameAsync(categoryName);

            return !result;
        }
    }
}
