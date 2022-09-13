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

        public async Task AddCategoryAsync(Category category)
        {
            await _context.Categories.AddAsync(category);

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

        public async Task<Guid> FindCategoryIdByCategoryNameAsync(string categoryName)
        {
            var categories = await _context.Categories.ToListAsync();

            Guid categoryId = (from category in categories
                               where category.Name == categoryName
                               select category.Id).FirstOrDefault();

            return categoryId;
        }
    }
}
