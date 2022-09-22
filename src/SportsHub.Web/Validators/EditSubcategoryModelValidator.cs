using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;

namespace SportsHub.Web.Validators
{
    public class EditSubcategoryModelValidator : AbstractValidator<EditSubcategoryModel>
    {
        private readonly ICategoryService _categoryService;
        private readonly ISubcategoryService _subcategoryService;

        public EditSubcategoryModelValidator(
            ICategoryService categoryService,
            ISubcategoryService subcategoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));

            RuleFor(subcategory => subcategory.Id)
                .NotEmpty().WithMessage(Errors.SubcategoryIdCannotBeEmpty)
                .MustAsync((id, cancellation) => _subcategoryService.DoesSubcategoryAlreadyExistByIdAsync(id))
                .WithMessage(Errors.SubcategoryDoesNotExist);

            RuleFor(subcategory => subcategory)
                .MustAsync((subcategory, cancellation) => DoesSubcategoryNameIsUniqueAsync(subcategory.Name, subcategory.Id))
                .WithMessage(Errors.SubcategoryNameIsNotUnique);

            RuleFor(subcategory => subcategory.Name)
                .NotEmpty().WithMessage(Errors.SubcategoryNameCannotBeEmpty);

            RuleFor(subcategory => subcategory.CategoryId)
                .NotEmpty().WithMessage(Errors.CategoryIdCannotBeEmpty)
                .MustAsync((id, cancellation) => _categoryService.DoesCategoryAlreadyExistByIdAsync(id))
                .WithMessage(Errors.CategoryDoesNotExist);
        }

        private async Task<bool> DoesSubcategoryNameIsUniqueAsync(string subcategoryName, Guid Id)
        {
            var result = await _subcategoryService.GetSubcategoryIdByNameAsync(subcategoryName);

            if (result == Guid.Empty)
            {
                return true;
            }
            else if (result == Id) { return true; }
            else return false;
        }
    }
}
