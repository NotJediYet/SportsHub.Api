using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
       
        Task<Category> GetCategoryByIdAsync(Guid id);
        
        Task CreateCategoryAsync(string categoryName);
        
        Task<bool> DoesCategoryAlreadyExistByNameAsync(string categoryName);

        Task<bool> DoesCategoryAlreadyExistByIdAsync(Guid id);

        Task<Guid> FindCategoryIdByCategoryNameAsync(string categoryName);
    }
}
