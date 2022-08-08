using SportsHub.Business.Repositories;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;
using Microsoft.EntityFrameworkCore;

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
            return await _context.Set<Subcategory>().ToListAsync();
        }

        public async Task<Subcategory> GetSubcategoryByIdAsync(Guid id)
        {
            return await _context.Set<Subcategory>().FindAsync(id);
        }

        public async Task AddSubcategoryAsync(Subcategory subcategory)
        {
            await _context.Set<Subcategory>().AddAsync(subcategory);

            await _context.SaveChangesAsync();
        }
        public async Task<bool> DoesSubcategoryAlreadyExistByNameAsync(string subcategoryName)
        {
            var subcategories = await _context.Set<Subcategory>().ToListAsync();

            return subcategories.Any(subcategory => subcategory.Name == subcategoryName);
        }

        public async Task<bool> DoesSubcategoryAlredyExistByIdAsync(Guid id)
        {
            var subcategories = await _context.Set<Subcategory>().ToListAsync();

            return subcategories.Any(subcategory => subcategory.Id == id);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
