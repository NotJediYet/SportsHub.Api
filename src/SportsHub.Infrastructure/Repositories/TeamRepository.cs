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

        public async Task AddTeamAsync(Team team)
        {
            await _context.Teams.AddAsync(team);

            await _context.SaveChangesAsync();
        }

        public async Task<Guid> GetTeamIdByNameAsync(string teamName)
        {
            var teams = await _context.Set<Team>().ToListAsync();
            var foundTeam = teams.Find(team => team.Name == teamName);

            if (foundTeam == null)
            {
                return Guid.Empty;
            }
            else return foundTeam.Id;
        }

        public async Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id)
        {
            var teams = await _context.Set<Team>().ToListAsync();

            return teams.Any(team => team.Id == id);
        }

        public async Task EditTeamAsync(Team team)
        {
            var oldTeam = await _context.Teams.FirstOrDefaultAsync(oldTeam => oldTeam.Id == team.Id);

            oldTeam.Name = team.Name;
            oldTeam.Location = team.Location;
            oldTeam.SubcategoryId = team.SubcategoryId;

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