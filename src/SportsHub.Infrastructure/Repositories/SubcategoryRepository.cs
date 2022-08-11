using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Repositories;

namespace SportsHub.Infrastructure.Repositories
{
    internal class SubcategoryRepository : ISubcategoryRepository
    {
        readonly protected SportsHubDbContext _context;

        public SubcategoryRepository(SportsHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Subcategory>> GetSubcategoriesAsync()
        {
            return await _context.Subcategories.ToListAsync();
        }

        public async Task<Subcategory> GetSubcategoryByIdAsync(Guid id)
        {
            return await _context.Subcategories.FindAsync(id);
        }

        public async Task AddSubcategoryAsync(Subcategory subcategory)
        {
            await _context.Subcategories.AddAsync(subcategory);

            await _context.SaveChangesAsync();
        }
        public async Task<bool> DoesSubcategoryAlreadyExistByNameAsync(string subcategoryName)
        {
            var subcategories = await _context.Subcategories.AnyAsync(subcategory => subcategory.Name == subcategoryName);

            return subcategories;
        }

        public async Task<bool> DoesSubcategoryAlredyExistByIdAsync(Guid id)
        {
            var subcategories = await _context.Subcategories.AnyAsync(subcategory => subcategory.Id == id);

            return subcategories;
        }
    }
}
