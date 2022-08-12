using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;

namespace SportsHub.Web.Validators
{
    public class CreateSubcategoryModelValidator : AbstractValidator<CreateSubcategoryModel>
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
                .NotEmpty().WithMessage(Errors.SubcategoryNameCannotBeEmpty)
                .MustAsync((name, cancellation) => DoesSubcategoryNameIsUniqueAsync(name))
                .WithMessage(Errors.SubcategoryNameIsNotUnique);

            RuleFor(subcategory => subcategory.CategoryId)
                .NotEmpty().WithMessage(Errors.CategoryIdCannotBeEmpty)
                .MustAsync((id, cancellation) => _categoryService.DoesCategoryAlredyExistByIdAsync(id))
                .WithMessage(Errors.CategoryDoesNotExist);
        }

        private async Task<bool> DoesSubcategoryNameIsUniqueAsync(string subcategoryName)
        {
            var result = await _subcategoryService.DoesSubcategoryAlreadyExistByNameAsync(subcategoryName);

            return !result;
        }
    }
}
