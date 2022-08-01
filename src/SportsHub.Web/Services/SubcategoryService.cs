using SportsHub.Web.Interfaces;
using SportsHub.Web.Models;

namespace SportsHub.Web.Services
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SubcategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Subcategory> GetSubcategories()
        {
            return _unitOfWork.Subcategories.Get().OrderByDescending(s => s.Id);
        }

        public Subcategory GetSubcategoryByID(int id)
        {
            Subcategory? subcategory = _unitOfWork.Subcategories.GetByID(id);
            if (subcategory == null)
            {
                throw new ApplicationException("Subcategory with that id is not found");
            }
            return subcategory;
        }

        public void CreateSubcategory(string newName, int categoryId)
        {
            if (newName == null)
            {
                throw new ApplicationException("Category name can't be null");
            }
            if (checkIfCategoryNotExists(categoryId))
            {
                throw new ApplicationException(
                    "Category with that id doesn't exists!");
            }
            if (chechIfNameNotUnique(newName))
            {
                throw new ApplicationException(
                    "Subcategory with that name already exists!");
            }
            _unitOfWork.Subcategories.Add(new Subcategory(newName, categoryId));
            _unitOfWork.Save();
        }
        private bool chechIfNameNotUnique(string newName)
        {
            return _unitOfWork.Subcategories.Get()
                    .Any(item => item.Name == newName);
        }

        private bool checkIfCategoryNotExists(int categoryId)
        {
            return _unitOfWork.Categories.GetByID(categoryId) == null;
        }
    }
}
