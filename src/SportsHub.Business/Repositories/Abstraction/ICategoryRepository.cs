using SportsHub.Shared.Entities;

namespace SportsHub.Repositories
{
    public interface ICategoryRepository 
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task<Category> GetCategoryByIdAsync(Guid id);

        Task AddCategoryAsync(Category category);

        Task<bool> DoesCategoryAlreadyExistByNameAsync(string categoryName);

        Task<bool> DoesCategoryAlredyExistByIdAsync(Guid id);
    }
}
