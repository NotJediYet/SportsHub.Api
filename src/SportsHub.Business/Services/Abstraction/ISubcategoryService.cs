using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    public interface ISubcategoryService
    {
        Task<IEnumerable<Subcategory>> GetSubcategoriesAsync();
        
        Task<Subcategory> GetSubcategoryByIdAsync(Guid id);
        
        Task CreateSubcategoryAsync(string subcategoryName, Guid categoryId);
        
        Task<bool> DoesSubcategoryAlreadyExistByNameAsync(string subcategoryName);

        Task<bool> DoesSubcategoryAlredyExistByIdAsync(Guid id);
    }
}
