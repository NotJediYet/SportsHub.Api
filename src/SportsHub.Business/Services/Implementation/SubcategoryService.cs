using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Services.Abstraction;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services.Implementation
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly SportsHubDbContext _context;

        public SubcategoryService(SportsHubDbContext context)
        {
            _context = context;
        }

        public async Task<List<Subcategory>> GetAllAsync()
        {
            return await _context.Subcategories.ToListAsync();
        }

        public async Task<Subcategory> GetByIdAsync(Guid id)
        {
            var subcategory = await _context.Subcategories.FirstOrDefaultAsync(
                subcategory => subcategory.Id == id);

            return subcategory;
        }

        public async Task CreateAsync(string newName, Guid categoryId)
        {
            await _context.Subcategories.AddAsync(
                new Subcategory(newName, categoryId));

            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfNameNotUniqueAsync(string newName)
        {
            return await _context.Subcategories.AnyAsync(
                subcategory => subcategory.Name == newName);
        }

        public async Task<bool> CheckIfCategoryIdNotExists(Guid id)
        {
            return (await _context.Categories.FirstOrDefaultAsync(
                category => category.Id == id) == null);
        }
    }
}
