using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    public interface ISubcategoryService
    {
        Task<IEnumerable<Subcategory>> GetAllAsync();
        
        Task<Subcategory> GetByIdAsync(Guid id);
        
        Task CreateAsync(string subcategoryName, Guid categoryId);
        
        Task<bool> DoesSubcategoryAlreadyExistByNameAsync(string subcategoryName);

        Task<bool> DoesSubcategoryAlredyExistByIdAsync(Guid id);
    }
}
