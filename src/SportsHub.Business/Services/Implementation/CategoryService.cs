using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Services.Abstraction;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly SportsHubDbContext _context;

        public CategoryService(SportsHubDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllAsync()
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
            return await _context.Categories.AnyAsync(
                category => category.Name == categoryName);
        }
    }
}
