using SportsHub.Shared.Entities;

namespace SportsHub.Business.Repositories
{
    public interface ICategoryRepository 
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task<Category> GetCategoryByIdAsync(Guid id);

        Task<Category> GetCategoryByNameAsync(string categoryName);

        Task AddCategoryAsync(Category category);

        Task<bool> DoesCategoryAlreadyExistByNameAsync(string categoryName);

        Task<bool> DoesCategoryAlreadyExistByIdAsync(Guid id);

        Task EditCategoryAsync(Category category);

        Task<Category> DeleteCategoryAsync(Guid id);

        Task<Guid> FindCategoryIdByCategoryNameAsync(string categoryName);
    }
}
