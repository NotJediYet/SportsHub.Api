using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
       
        Task<Category> GetByIdAsync(Guid id);
        
        Task CreateAsync(string categoryName);
        
        Task<bool> DoesCategoryAlreadyExistByNameAsync(string categoryName);

        Task<bool> DoesCategoryAlredyExistByIdAsync(Guid id);
    }
}
