using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Services.Abstraction;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services.Implementation
{
    public class TeamService : ITeamService
    {
        private readonly SportsHubDbContext _context;

        public TeamService(SportsHubDbContext Context)
        {
            _context = Context;
        }
       
        public async Task<List<Team>> GetAllAsync()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team?> GetByIDAsync(Guid Id)
        {
            return await _context.Teams.Where(c => c.Id.Equals(Id)).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Team Team)
        {
            await _context.Teams.AddAsync(Team);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CheckIfIdNotUniqueAsync(Guid Id)
        {
            return await _context.Teams.AnyAsync(item => item.Id == Id);
        }

        public async Task<bool> CheckIfNameNotUniqueAsync(string NewName)
        {
            return await _context.Teams.AnyAsync(item => item.Name == NewName);
        }

        public async Task<bool> CheckIfSubcategoryIdNotExists(Guid Id)
        {
            return (await _context.Subcategories.Where(c => c.Id.Equals(Id)).FirstOrDefaultAsync() == null);
        }
    }
}
