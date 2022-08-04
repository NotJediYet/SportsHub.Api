using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services.Abstraction
{
    public interface ISubcategoryService
    {
        Task<List<Subcategory>> GetAllAsync();
        
        Task<Subcategory> GetByIdAsync(Guid id);
        
        Task CreateAsync(string newName, Guid categoryId);
        
        Task<bool> CheckIfNameNotUniqueAsync(string newName);
        
        Task<bool> CheckIfCategoryIdNotExists(Guid id);
    }
}
