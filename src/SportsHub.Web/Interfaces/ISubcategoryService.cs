using SportsHub.Web.Models;


namespace SportsHub.Web.Interfaces
{
    public interface ISubcategoryService
    {
        IEnumerable<Subcategory> GetSubcategories();
        Subcategory GetSubcategoryByID(int id);
        void CreateSubcategory(string newName, int categoryId);
    }
}
