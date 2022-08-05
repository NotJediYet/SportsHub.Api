using Microsoft.EntityFrameworkCore;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    internal class CategoryService : ICategoryService
    {
        private readonly SportsHubDbContext _context;

        public CategoryService(SportsHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetByIdAsync(Guid id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(
                category => category.Id == id);

            return category;
        }

        public async Task CreateAsync(string categoryName)
        {
            await _context.Categories.AddAsync(
                new Category(categoryName));

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesCategoryAlreadyExistByNameAsync(string categoryName)
        {
            var result = await _context.Categories.AnyAsync(
                category => category.Name == categoryName);

            return result;
        }

        public async Task<bool> DoesCategoryAlredyExistByIdAsync(Guid id)
        {
            var result = await _context.Categories.AnyAsync(
                category => category.Id == id);

            return result;
        }
    }
}
