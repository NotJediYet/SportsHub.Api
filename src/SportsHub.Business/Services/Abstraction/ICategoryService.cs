using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services.Abstraction
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
       
        Task<Category> GetByIdAsync(Guid id);
        
        Task CreateAsync(string newName);
        
        Task<bool> CheckIfNameNotUniqueAsync(string newName);
    }
}
