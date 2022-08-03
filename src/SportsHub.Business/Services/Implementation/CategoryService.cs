using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Services.Abstraction;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly SportsHubDbContext _context;

        public CategoryService(SportsHubDbContext Context)
        {
            _context = Context;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIDAsync(Guid Id)
        {
            return await _context.Categories.Where(c => c.Id.Equals(Id)).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Category Category)
        {
            await _context.Categories.AddAsync(Category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfNameNotUniqueAsync(string NewName)
        {
            return await _context.Categories.AnyAsync(item => item.Name == NewName);
        }

        public async Task<bool> CheckIfIdNotUniqueAsync(Guid Id)
        {
            return await _context.Categories.AnyAsync(item => item.Id == Id);
        }
    }
}
