using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Repositories;

namespace SportsHub.Infrastructure.Repositories
{
    internal class TeamRepository : ITeamRepository
    {
        readonly protected SportsHubDbContext _context;

        public TeamRepository(SportsHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Team>> GetTeamsAsync()
        {
            return await _context.Teams.ToListAsync();
        }

        public async Task<Team> GetTeamByIdAsync(Guid id)
        {
            return await _context.Teams.FindAsync(id);
        }

        public async Task AddTeamAsync(Team team)
        {
            await _context.Teams.AddAsync(team);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName)
        {
            return await _context.Teams.AnyAsync(team => team.Name == teamName);
        }
        public async Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id)
        {
            return await _context.Teams.AnyAsync(team => team.Id == id);
        }
    }
}
