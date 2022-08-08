using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;

namespace SportsHub.Web.Validators
{
    internal class CreateSubcategoryModelValidator : AbstractValidator<CreateSubcategoryModel>
    {
        private readonly ICategoryService _categoryService;
        private readonly ISubcategoryService _subcategoryService;

        public CreateSubcategoryModelValidator(
            ICategoryService categoryService,
            ISubcategoryService subcategoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));

            RuleFor(subcategory => subcategory.Name)
                .NotEmpty().WithMessage("Subcategory name can not be empty!")
                .MustAsync((name, cancellation) => DoesSubcategoryNameIsUniqueAsync(name))
                .WithMessage("Subcategory with that name already exists!");

            RuleFor(subcategory => subcategory.CategoryId)
                .NotEmpty().WithMessage("Category id can not be empty!")
                .MustAsync((id, cancellation) => DoesCategoryExistByIdAsync(id)).WithMessage("Category with that id does not exist!");
        }

        private async Task<bool> DoesSubcategoryNameIsUniqueAsync(string subcategoryName)
        {
            var result = await _subcategoryService.DoesSubcategoryAlreadyExistByNameAsync(subcategoryName);

            return !result;
        }

        private async Task<bool> DoesCategoryExistByIdAsync(Guid id)
        {
            var result = await _categoryService.DoesCategoryAlredyExistByIdAsync(id);

            return result;
        }
    }
}
