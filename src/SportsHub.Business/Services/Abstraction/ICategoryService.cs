using SportsHub.Shared.Models;

namespace SportsHub.Business.Services.Abstraction
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllAsync();
        Task<Category?> GetByIDAsync(Guid Id);
        Task CreateAsync(Category Category);
        Task<bool> CheckIfNameNotUniqueAsync(string NewName);
        Task<bool> CheckIfIdNotUniqueAsync(Guid Id);
    }
}
