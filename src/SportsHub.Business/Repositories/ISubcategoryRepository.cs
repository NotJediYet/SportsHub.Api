﻿using SportsHub.Shared.Entities;

namespace SportsHub.Business.Repositories
{
    public interface ISubcategoryRepository
    {
        Task<IEnumerable<Subcategory>> GetSubcategoriesAsync();

        Task<Subcategory> GetSubcategoryByIdAsync(Guid id);

        Task AddSubcategoryAsync(Subcategory subcategory);

        Task<bool> DoesSubcategoryAlreadyExistByNameAsync(string subcategoryName);

        Task<bool> DoesSubcategoryAlredyExistByIdAsync(Guid id);
    }
}