using SportsHub.Business.Repositories;
using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    internal class SubcategoryService : ISubcategoryService
    {
        private readonly ISubcategoryRepository _subcategoryRepository;

        public SubcategoryService(ISubcategoryRepository subcategoryRepository)
        {
            _subcategoryRepository = subcategoryRepository ?? throw new ArgumentNullException(nameof(subcategoryRepository));
        }

        public async Task<IEnumerable<Subcategory>> GetSubcategoriesAsync()
        {
            return await _subcategoryRepository.GetAllAsync();
        }

        public async Task<Subcategory> GetSubcategoryByIdAsync(Guid id)
        {
            var subcategory = await _subcategoryRepository.GetByIdAsync(id);

            return subcategory;
        }

        public async Task CreateSubcategoryAsync(string subcategoryName, Guid categoryId)
        {
            await _subcategoryRepository.AddAsync(new Subcategory(subcategoryName, categoryId));

            await _subcategoryRepository.SaveAsync();
        }

        public async Task<bool> DoesSubcategoryAlreadyExistByNameAsync(string subcategoryName)
        {
            var subcategories = await _subcategoryRepository.GetAllAsync();

            return subcategories.Any(subcategory => subcategory.Name == subcategoryName);
        }

        public async Task<bool> DoesSubcategoryAlredyExistByIdAsync(Guid id)
        {
            var subcategories = await _subcategoryRepository.GetAllAsync();

            return subcategories.Any(subcategory => subcategory.Id == id);
        }
    }
}
