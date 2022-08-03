using SportsHub.Shared.Models;

namespace SportsHub.Business.Services.Abstraction
{
    public interface ISubcategoryService
    {
        Task<List<Subcategory>> GetAllAsync();
        Task<Subcategory?> GetByIDAsync(Guid Id);
        Task CreateAsync(Subcategory Subcategory);
        Task<bool> CheckIfNameNotUniqueAsync(string NewName);
        Task<bool> CheckIfIdNotUniqueAsync(Guid Id);
        Task<bool> CheckIfCategoryIdNotExists(Guid Id);
    }
}
