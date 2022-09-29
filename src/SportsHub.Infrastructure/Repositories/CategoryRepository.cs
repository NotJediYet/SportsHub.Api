using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Repositories;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;

namespace SportsHub.Infrastructure.Repositories
{
    internal class CategoryRepository : ICategoryRepository
    {
        readonly protected SportsHubDbContext _context;

        public CategoryRepository(SportsHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> GetCategoryByNameAsync(string categoryName)
        {
            return await _context.Categories.FirstOrDefaultAsync(category => category.Name == categoryName);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);

            await _context.SaveChangesAsync();
        }

        public async Task EditCategoryAsync(Category category)
        {
            var oldCategory = await _context.Categories.FirstOrDefaultAsync(oldCategory => oldCategory.Id == category.Id);

            oldCategory.Name = category.Name;
            oldCategory.IsStatic = category.IsStatic;
            oldCategory.IsHidden = category.IsHidden;
            oldCategory.OrderIndex = category.OrderIndex;

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesCategoryAlreadyExistByNameAsync(string categoryName)
        {
            var categories = await _context.Categories.AnyAsync(category => category.Name == categoryName);

            return categories;
        }

        public async Task<bool> DoesCategoryAlreadyExistByIdAsync(Guid id)
        {
            var categories = await _context.Categories.AnyAsync(category => category.Id == id);

            return categories;
        }

        public async Task<Category> DeleteCategoryAsync(Guid id)
        {
            var category = _context.Categories.Find(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }

            return category;
        }
        
        public async Task<Guid> FindCategoryIdByCategoryNameAsync(string categoryName)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(category => category.Name == categoryName);
            return category.Id;
        }
    }
}
