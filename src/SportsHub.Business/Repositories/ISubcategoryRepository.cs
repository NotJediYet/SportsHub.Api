using SportsHub.Shared.Entities;

namespace SportsHub.Business.Repositories
{
    public interface ISubcategoryRepository
    {
        Task<IEnumerable<Subcategory>> GetSubcategoriesAsync();

        Task<Subcategory> GetSubcategoryByIdAsync(Guid id);

        Task<Subcategory> GetSubcategoryByNameAsync(string subcategoryName);

        Task AddSubcategoryAsync(Subcategory subcategory);

        Task<bool> DoesSubcategoryAlreadyExistByNameAsync(string subcategoryName);

        Task<bool> DoesSubcategoryAlreadyExistByIdAsync(Guid id);

        Task<Guid> FindSubcategoryIdBySubcategoryNameAsync(string subcategoryName);

        Task EditSubcategoryAsync(Subcategory subcategory);

        Task<IEnumerable<Subcategory>> GetByCategoryIdAsync(Guid categoryId);
    }
}
