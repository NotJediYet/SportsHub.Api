﻿using SportsHub.Shared.Entities;

namespace SportsHub.Business.Repositories
{
    public interface ICategoryRepository 
    {
        Task<IEnumerable<Category>> GetCategoriesAsync();

        Task<Category> GetCategoryByIdAsync(Guid id);

        Task AddCategoryAsync(Category category);

        Task<Guid> GetCategoryIdByNameAsync(string categoryName);

        Task<bool> DoesCategoryAlreadyExistByNameAsync(string categoryName);

        Task<bool> DoesCategoryAlreadyExistByIdAsync(Guid id);

        Task EditCategoryAsync(Category category);
    }
}
