using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;

namespace SportsHub.Web.Validators
{
    public class EditCategoryModelValidator : AbstractValidator<EditCategoryModel>
    {
        private readonly ICategoryService _categoryService;

        public EditCategoryModelValidator(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));

            RuleFor(category => category.Id)
                .NotEmpty().WithMessage(Errors.CategoryIdCannotBeEmpty)
                .MustAsync((id, cancellation) => _categoryService.DoesCategoryAlreadyExistByIdAsync(id))
                .WithMessage(Errors.CategoryDoesNotExist);

            RuleFor(category => category)
                .MustAsync((category, cancellation) => DoesCategoryNameIsUniqueAsync(category.Name, category.Id))
                .WithMessage(Errors.CategoryNameIsNotUnique);

            RuleFor(category => category.Name)
                .NotEmpty().WithMessage(Errors.CategoryNameCannotBeEmpty);
        }

        private async Task<bool> DoesCategoryNameIsUniqueAsync(string categoryName, Guid Id)
        {
            var result = await _categoryService.GetCategoryIdByNameAsync(categoryName);

            if (result == Guid.Empty)
            {
                return true;
            }
            else if (result == Id) { return true; }
            else return false;
        }
    }
}
