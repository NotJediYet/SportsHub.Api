using Microsoft.EntityFrameworkCore;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    internal class TeamService : ITeamService
    {
        private readonly SportsHubDbContext _context;

        public TeamService(SportsHubDbContext context)
        {
            _context = context;
        }
       
        public async Task<IEnumerable<Team>> GetAllAsync()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team> GetByIdAsync(Guid id)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(
                team => team.Id == id);

            return team;
        }

        public async Task CreateAsync(string teamName, Guid subcategoryId)
        {
            await _context.Teams.AddAsync(
                new Team(teamName, subcategoryId));

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName)
        {
            var result = await _context.Teams.AnyAsync(
                team => team.Name == teamName);

            return result;
        }
    }
}
