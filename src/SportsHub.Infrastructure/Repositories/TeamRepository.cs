using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Repositories;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

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

        public async Task<Team> GetTeamByNameAsync(string teamName)
        {
            return await _context.Teams.FirstOrDefaultAsync(team => team.Name == teamName);
        }

        public async Task AddTeamAsync(Team team)
        {
            await _context.Teams.AddAsync(team);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id)
        {
            var teams = await _context.Teams.ToListAsync();

            return teams.Any(team => team.Id == id);
        }

        public async Task EditTeamAsync(Team team)
        {
            var oldTeam = await _context.Teams.FirstOrDefaultAsync(oldTeam => oldTeam.Id == team.Id);

            oldTeam.Name = team.Name;
            oldTeam.Location = team.Location;
            oldTeam.SubcategoryId = team.SubcategoryId;
            oldTeam.IsHidden = team.IsHidden;
            oldTeam.OrderIndex = team.OrderIndex;

            await _context.SaveChangesAsync();
        }

        public async Task<Guid> FindTeamIdBySubcategoryIdAsync(Guid subcategoryId)
        {
            var teams = await _context.Teams.ToListAsync();

            Guid teamId = (from team in teams
                           where team.SubcategoryId == subcategoryId
                           select team.Id).FirstOrDefault();

            return teamId;
        }

        public async Task<bool> DoesTeamAlredyExistByIdAsync(Guid id)
        {
            var teams = await _context.Set<Team>().ToListAsync();

            return teams.Any(team => team.Id == id);
        }
    }
}