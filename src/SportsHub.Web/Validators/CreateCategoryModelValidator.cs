using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;

namespace SportsHub.Validators
{
    internal class CreateCategoryModelValidator : AbstractValidator<CreateCategoryModel>
    {
        private readonly ICategoryService _categoryService;

        public CreateCategoryModelValidator(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));

            RuleFor(category => category.Name)
                .NotEmpty().WithMessage("Category name can not be empty!")
                .MustAsync((name, cancellation) => DoesCategoryNameIsUniqueAsync(name))
                .WithMessage("Category with that name already exists!");
        }

        private async Task<bool> DoesCategoryNameIsUniqueAsync(string categoryName)
        {
            var result = await _categoryService.DoesCategoryAlreadyExistByNameAsync(categoryName);

            return !result;
        }
    }
}
