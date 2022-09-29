using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();
       
        Task<Category> GetCategoryByIdAsync(Guid id);
        
        Task CreateCategoryAsync(string categoryName);

        Task<Guid> GetCategoryIdByNameAsync(string categoryName);

        Task<bool> DoesCategoryAlreadyExistByNameAsync(string categoryName);

        Task<bool> DoesCategoryAlreadyExistByIdAsync(Guid id);

        Task EditCategoryAsync(EditCategoryModel editCategoryModel);

        Task<Category> DeleteCategoryAsync(Guid Id);
    }
}
