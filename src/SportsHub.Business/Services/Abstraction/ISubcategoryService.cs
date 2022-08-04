using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services.Abstraction
{
    public interface ISubcategoryService
    {
        Task<List<Subcategory>> GetAllAsync();
        
        Task<Subcategory> GetByIdAsync(Guid id);
        
        Task CreateAsync(string subcategoryName, Guid categoryId);
        
        Task<bool> DoesSubcategoryAlreadyExistByNameAsync(string subcategoryName);
        
        Task<bool> DoesCategoryAlredyExistByIdAsync(Guid id);
    }
}
