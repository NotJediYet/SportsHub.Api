using SportsHub.Shared.Entities;

namespace SportsHub.Repositories
{
    public interface ISubcategoryRepository
    {
        Task<IEnumerable<Subcategory>> GetSubcategoriesAsync();

        Task<Subcategory> GetSubcategoryByIdAsync(Guid id);

        Task AddSubcategoryAsync(Subcategory subcategory);

        Task<bool> DoesSubcategoryAlreadyExistByNameAsync(string subcategoryName);

        Task<bool> DoesSubcategoryAlredyExistByIdAsync(Guid id);
    }
}
