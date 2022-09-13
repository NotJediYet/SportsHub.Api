﻿using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    public interface ISubcategoryService
    {
        Task<IEnumerable<Subcategory>> GetSubcategoriesAsync();
        
        Task<Subcategory> GetSubcategoryByIdAsync(Guid id);
        
        Task CreateSubcategoryAsync(string subcategoryName, Guid categoryId);
        
        Task<bool> DoesSubcategoryAlreadyExistByNameAsync(string subcategoryName);

        Task<bool> DoesSubcategoryAlreadyExistByIdAsync(Guid id);

        Task<Guid> FindSubcategoryIdBySubcategoryNameAsync(string subcategoryName);

        IQueryable<Subcategory> GetSubcategoryIdByCategoryIdAsync(Guid categoryId);
    }
}
