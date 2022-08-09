using SportsHub.Repositories;
using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    internal class CategoryService : ICategoryService
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
            var category = await _categoryRepository.GetCategoryByIdAsync(id);

            return category;
        }

        public async Task CreateCategoryAsync(string categoryName)
        {
            await _categoryRepository.AddCategoryAsync(new Category(categoryName));
        }

        public async Task<bool> DoesCategoryAlreadyExistByNameAsync(string categoryName)
        {
            var result = await _categoryRepository.DoesCategoryAlreadyExistByNameAsync(categoryName);

            return result;
        }

        public async Task<bool> DoesCategoryAlredyExistByIdAsync(Guid id)
        {
            var result = await _categoryRepository.DoesCategoryAlredyExistByIdAsync(id);

            return result;
        }
    }
}
