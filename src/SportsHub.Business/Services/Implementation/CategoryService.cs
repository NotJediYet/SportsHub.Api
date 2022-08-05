using SportsHub.Business.Repositories;
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
            return await _categoryRepository.GetAllAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);

            return category;
        }

        public async Task CreateCategoryAsync(string categoryName)
        {
            await _categoryRepository.AddAsync(new Category(categoryName));

            await _categoryRepository.SaveAsync();
        }

        public async Task<bool> DoesCategoryAlreadyExistByNameAsync(string categoryName)
        {
            var categories = await _categoryRepository.GetAllAsync();
            
            return categories.Any(category => category.Name == categoryName);
        }

        public async Task<bool> DoesCategoryAlredyExistByIdAsync(Guid id)
        {
            var categories = await _categoryRepository.GetAllAsync();

            return categories.Any(category => category.Id == id);
        }
    }
}
