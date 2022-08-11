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
            return await _subcategoryRepository.GetSubcategoriesAsync();
        }

        public async Task<Subcategory> GetSubcategoryByIdAsync(Guid id)
        {
            var subcategory = await _subcategoryRepository.GetSubcategoryByIdAsync(id);

            return subcategory;
        }

        public async Task CreateSubcategoryAsync(string subcategoryName, Guid categoryId)
        {
            await _subcategoryRepository.AddSubcategoryAsync(new Subcategory(subcategoryName, categoryId));
        }

        public async Task<bool> DoesSubcategoryAlreadyExistByNameAsync(string subcategoryName)
        {
            var result = await _subcategoryRepository.DoesSubcategoryAlreadyExistByNameAsync(subcategoryName);

            return result;
        }

        public async Task<bool> DoesSubcategoryAlreadyExistByIdAsync(Guid id)
        {
            var result = await _subcategoryRepository.DoesSubcategoryAlreadyExistByIdAsync(id);

            return result;
        }
    }
}
