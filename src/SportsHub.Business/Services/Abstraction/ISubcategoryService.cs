using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public interface ISubcategoryService
    {
        Task<IEnumerable<Subcategory>> GetSubcategoriesAsync();
        
        Task<Subcategory> GetSubcategoryByIdAsync(Guid id);
        
        Task CreateSubcategoryAsync(string subcategoryName, Guid categoryId);

        Task<Guid> GetSubcategoryIdByNameAsync(string subcategoryName);

        Task<bool> DoesSubcategoryAlreadyExistByNameAsync(string subcategoryName);

        Task<bool> DoesSubcategoryAlreadyExistByIdAsync(Guid id);   

        Task<Guid> FindSubcategoryIdBySubcategoryNameAsync(string subcategoryName);

        Task EditSubcategoryAsync(EditSubcategoryModel editSubcategoryModel);
    }
}
