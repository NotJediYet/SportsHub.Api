using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Repositories;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;
using System.Linq;

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

        public async Task<Guid> FindTeamIdByTeamNameAsync(string teamName)
        {
            var teams = await _context.Teams.ToListAsync();

            Guid teamId = (from team in teams
                           where team.Name == teamName
                           select team.Id).FirstOrDefault();

            return teamId;
        }

        public async Task<Guid> FindTeamIdBySubcategoryIdAsync(Guid subcategoryId)
        {
            var teams = await _context.Teams.ToListAsync();

            Guid teamId = (from team in teams
                           where team.SubcategoryId == subcategoryId
                           select team.Id).FirstOrDefault();

            return teamId;
        }

        public List<Team> GetTeamsFilteredByLocation(string location, List<Team> teams)
        {

            return teams.Where(teams => teams.Location == location).ToList();
        }

        public List<Team> GetTeamsFilteredBySubcategoryId(Guid subcategoryId, List<Team> teams)
        {

            return teams.Where(teams => teams.SubcategoryId == subcategoryId).ToList();
        }

        public List<Team> GetTeamsFilteredBySubcategoryIds(List<Guid> subcategoryIds, List<Team> teams)
        {
            var newTeam = teams;
            teams = newTeam.Where(team => team.SubcategoryId == subcategoryIds[0]).ToList();

            for (int i = 1; i < subcategoryIds.Count; i++)
            {
                teams = teams.Concat(newTeam.Where(team => team.SubcategoryId == subcategoryIds[i]).ToList()).ToList();
            }

            return teams;
        }

        public async Task<List<Team>> GetSortedTeamAsync()
        {
            return await _context.Teams.OrderBy(teams => teams.Name).ToListAsync();
        }
    }
}
