using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Repositories;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;

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
            return await _context.Set<Team>().ToListAsync();
        }

        public async Task<Team> GetTeamByIdAsync(Guid id)
        {
            return await _context.Set<Team>().FindAsync(id);
        }

        public async Task AddTeamAsync(Team team)
        {
            await _context.Set<Team>().AddAsync(team);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName)
        {
            var teams = await _context.Set<Team>().ToListAsync();

            return teams.Any(team => team.Name == teamName);
        }

        public async Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id)
        {
            var teams = await _context.Set<Team>().ToListAsync();

            return teams.Any(team => team.Id == id);
        }

        public async Task<bool> DoesTeamLogoAlreadyExistByTeamIdAsync(Guid teamId)
        {
            var teamLogos = await _context.Set<TeamLogo>().ToListAsync();

            return teamLogos.Any(teamLogo => teamLogo.TeamId == teamId);
        }

        public async Task<bool> DoesTeamLogoAlreadySatisfyConditionsAsync(Team team)
        {
            var formFile = team.TeamLogo;
            var fileExtension = Path.GetExtension(formFile.FileName);
            var fileSize = formFile.Length;

            if (fileSize < 2097152)
                return true;
            else if (fileExtension == ".jpg" || fileExtension == ".png" || fileExtension == ".svg")
                return true;
            else
                return false;
        }
    }
}
