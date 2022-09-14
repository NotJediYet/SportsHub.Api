using SportsHub.Business.Repositories;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetCategoriesAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            return await _categoryRepository.GetCategoryByIdAsync(id);
        }

        public async Task CreateCategoryAsync(string categoryName)
        {
            await _categoryRepository.AddCategoryAsync(new Category { Name = categoryName });
        }

        public async Task<Guid> GetCategoryIdByNameAsync(string categoryName)
        {
            return await _categoryRepository.GetCategoryIdByNameAsync(categoryName);
        }

        public async Task EditCategoryAsync(EditCategoryModel editCategoryModel)
        {
            var categoryModel = new Category
            {
                Id = editCategoryModel.Id,
                Name = editCategoryModel.Name,
                IsStatic = editCategoryModel.IsStatic,
                IsHidden = editCategoryModel.IsHidden,
                OrderIndex = editCategoryModel.OrderIndex
            };

            await _categoryRepository.EditCategoryAsync(categoryModel);
        }

        public async Task<bool> DoesCategoryAlreadyExistByNameAsync(string categoryName)
        {
            return await _categoryRepository.DoesCategoryAlreadyExistByNameAsync(categoryName);

        }

        public async Task<bool> DoesCategoryAlreadyExistByIdAsync(Guid id)
        {
            return await _categoryRepository.DoesCategoryAlreadyExistByIdAsync(id);
        }
    }
}
