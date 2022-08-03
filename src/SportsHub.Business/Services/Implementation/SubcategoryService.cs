using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Services.Abstraction;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services.Implementation
{
    public class SubcategoryService : ISubcategoryService
    {
        private readonly SportsHubDbContext _context;

        public SubcategoryService(SportsHubDbContext Context)
        {
            _context = Context;
        }

        public async Task<List<Subcategory>> GetAllAsync()
        {
            return await _context.Subcategories.ToListAsync();
        }

        public async Task<Subcategory?> GetByIDAsync(Guid Id)
        {
            return await _context.Subcategories.Where(c => c.Id.Equals(Id)).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Subcategory Subcategory)
        {
            await _context.Subcategories.AddAsync(Subcategory);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfIdNotUniqueAsync(Guid Id)
        {
            return await _context.Subcategories.AnyAsync(item => item.Id == Id);
        }

        public async Task<bool> CheckIfNameNotUniqueAsync(string NewName)
        {
            return await _context.Subcategories.AnyAsync(item => item.Name == NewName);
        }

        public async Task<bool> CheckIfCategoryIdNotExists(Guid Id)
        {
            return (await _context.Categories.Where(c => c.Id.Equals(Id)).FirstOrDefaultAsync() == null);
        }
    }
}
