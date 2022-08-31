using SportsHub.Business.Repositories;
using SportsHub.Shared.Entities;

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
            var category = new Category(categoryName);

            await _categoryRepository.AddCategoryAsync(category);
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
