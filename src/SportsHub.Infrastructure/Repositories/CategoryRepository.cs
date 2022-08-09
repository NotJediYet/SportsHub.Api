using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Repositories;

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
            return await _context.Set<Category>().ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(Guid id)
        {
            return await _context.Set<Category>().FindAsync(id);
        }

        public async Task AddCategoryAsync(Category category)
        {
            await _context.Set<Category>().AddAsync(category);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesCategoryAlreadyExistByNameAsync(string categoryName)
        {
            var categories = await _context.Set<Category>().ToListAsync();

            return categories.Any(category => category.Name == categoryName);
        }

        public async Task<bool> DoesCategoryAlredyExistByIdAsync(Guid id)
        {
            var categories = await _context.Set<Category>().ToListAsync();

            return categories.Any(category => category.Id == id);
        }
    }
}
