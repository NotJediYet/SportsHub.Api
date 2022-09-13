using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Repositories;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;

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

        public async Task<bool> DoesSubcategoryAlreadyExistByIdAsync(Guid id)
        {
            var subcategories = await _context.Subcategories.AnyAsync(subcategory => subcategory.Id == id);

            return subcategories;
        }

        public async Task<Guid> FindSubcategoryIdBySubcategoryNameAsync(string subcategoryName)
        {
            var subcategories = await _context.Subcategories.ToListAsync();

            Guid subcategoryId = (from subcategory in subcategories
                                  where subcategory.Name == subcategoryName
                                  select subcategory.Id).FirstOrDefault();

            return subcategoryId;
        }

        public async Task<List<Guid>> FindSubcategoryIdByCategoryIdAsync(Guid idCategory)
        {
            var subcategories = await _context.Subcategories.ToListAsync();

            var subcategoryIds = (from subcategory in subcategories
                                  where subcategory.CategoryId == idCategory
                                  select subcategory.Id).ToList();

            return subcategoryIds;
        }
    }
}