using Microsoft.AspNetCore.Mvc;
using SportsHub.Web.Interfaces;
using SportsHub.Web.Models;

namespace SportsHub.Web.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Category> GetCategories()
        {
            return _unitOfWork.Categories.Get().OrderByDescending(c => c.Id);
        }

        public Category GetCategoryByID(int id)
        {
            Category? category = _unitOfWork.Categories.GetByID(id);
            if (category == null)
            {
                throw new ApplicationException("Category with that id is not found");
            }
            return category;
        }

        public void CreateCategory(string newName)
        {
            if (newName == null)
            {
                throw new ApplicationException("Category name can't be null");
            }
            if (chechIfNameNotUnique(newName))
            {
                throw new ApplicationException(
                    "Category with that name already exists!");
            }
            _unitOfWork.Categories.Add(new Category(newName));
            _unitOfWork.Save();
        }

        private bool chechIfNameNotUnique(string newName)
        {
            return _unitOfWork.Categories.Get()
                    .Any(item => item.Name == newName);
        }
    }
}
