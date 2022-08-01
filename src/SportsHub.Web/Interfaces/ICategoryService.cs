using Microsoft.AspNetCore.Mvc;
using SportsHub.Web.Models;

namespace SportsHub.Web.Interfaces
{
    public interface ICategoryService
    {
        IEnumerable<Category> GetCategories();
        Category GetCategoryByID(int id);
        void CreateCategory(string newName);
    }
}
