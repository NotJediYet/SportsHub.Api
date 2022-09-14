using SportsHub.Business.Repositories;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public class SubcategoryService : ISubcategoryService
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
            return await _subcategoryRepository.GetSubcategoryByIdAsync(id);
        }

        public async Task CreateSubcategoryAsync(string subcategoryName, Guid categoryId)
        {
            await _subcategoryRepository.AddSubcategoryAsync(new Subcategory { Name = subcategoryName, CategoryId = categoryId });
        }

        public async Task<Guid> GetSubcategoryIdByNameAsync(string subcategoryName)
        {
            return await _subcategoryRepository.GetSubcategoryIdByNameAsync(subcategoryName);
        }

        public async Task EditSubcategoryAsync(EditSubcategoryModel editSubcategoryModel)
        {
            var subcategoryModel = new Subcategory
            {
                Id = editSubcategoryModel.Id,
                Name = editSubcategoryModel.Name,
                CategoryId = editSubcategoryModel.CategoryId,
                IsHidden = editSubcategoryModel.IsHidden,
                OrderIndex = editSubcategoryModel.OrderIndex
            };

            await _subcategoryRepository.EditSubcategoryAsync(subcategoryModel);
        }

        public async Task<bool> DoesSubcategoryAlreadyExistByNameAsync(string subcategoryName)
        {
            return await _subcategoryRepository.DoesSubcategoryAlreadyExistByNameAsync(subcategoryName);
        }

        public async Task<bool> DoesSubcategoryAlreadyExistByIdAsync(Guid id)
        {
            return await _subcategoryRepository.DoesSubcategoryAlreadyExistByIdAsync(id);
        }

        public async Task<Guid> FindSubcategoryIdBySubcategoryNameAsync(string subcategoryName)
        {
            return await _subcategoryRepository.FindSubcategoryIdBySubcategoryNameAsync(subcategoryName);
        }
    }
}
