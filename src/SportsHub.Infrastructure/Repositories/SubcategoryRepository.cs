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

        public async Task<Subcategory> GetSubcategoryByNameAsync(string subcategoryName)
        {
            return await _context.Subcategories.FirstOrDefaultAsync(subcategory => subcategory.Name == subcategoryName);
        }

        public async Task AddSubcategoryAsync(Subcategory subcategory)
        {
            await _context.Subcategories.AddAsync(subcategory);

            await _context.SaveChangesAsync();
        }

        public async Task EditSubcategoryAsync(Subcategory subcategory)
        {
            var oldSubcategory = await _context.Subcategories.FirstOrDefaultAsync(oldSubcategory => oldSubcategory.Id == subcategory.Id);

            oldSubcategory.Name = subcategory.Name;
            oldSubcategory.CategoryId = subcategory.CategoryId;
            oldSubcategory.IsHidden = subcategory.IsHidden;
            oldSubcategory.OrderIndex = subcategory.OrderIndex;

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

        public async Task<Subcategory> DeleteSubcategoryAsync(Guid id)
        {
            var subcategory = _context.Subcategories.Find(id);
            if (subcategory != null)
            {
                _context.Subcategories.Remove(subcategory);
                await _context.SaveChangesAsync();
            }

            return subcategory;
        }
        
        public async Task<IEnumerable<Subcategory>> GetSubcategoriesByCategoryIdAsync(Guid categoryId)
        {
            return await _context.Subcategories.Where(subcategory => subcategory.CategoryId == categoryId).ToListAsync();
        }
    }
}