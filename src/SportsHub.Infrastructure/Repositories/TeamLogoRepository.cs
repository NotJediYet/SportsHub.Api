using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Repositories;
using Microsoft.AspNetCore.Http;

namespace SportsHub.Infrastructure.Repositories
{
    internal class TeamLogoRepository : ITeamLogoRepository
    {
        readonly protected SportsHubDbContext _context;

        public TeamLogoRepository(SportsHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TeamLogo>> GetTeamLogosAsync()
        {
            return await _context.Set<TeamLogo>().ToListAsync();
        }

        public async Task<TeamLogo> GetTeamLogoByIdAsync(Guid id)
        {
            return await _context.Set<TeamLogo>().FindAsync(id);
        }

        public async Task AddTeamLogoAsync(IFormFile teamLogoFile, Guid teamId)
        {
            var memoryStream = new MemoryStream();
            await teamLogoFile.CopyToAsync(memoryStream);

            var fileBytes = memoryStream.ToArray();
            var fileExtension = Path.GetExtension(teamLogoFile.FileName);
            var fileSize = teamLogoFile.Length;
            TeamLogo newTeamLogo = new TeamLogo(fileBytes, fileExtension, fileSize, teamId);

            await _context.Set<TeamLogo>().AddAsync(newTeamLogo);
            await _context.SaveChangesAsync();
        }

    public async Task<bool> DoesTeamLogoAlreadyExistByTeamIdAsync(Guid teamId)
        {
            var teamLogos = await _context.Set<TeamLogo>().ToListAsync();

            return teamLogos.Any(teamLogo => teamLogo.TeamId == teamId);
        }

        public async Task<bool> DoesTeamLogoAlreadyExistByIdAsync(Guid id)
        {
            var teamLogos = await _context.Set<TeamLogo>().ToListAsync();

            return teamLogos.Any(teamLogo => teamLogo.Id == id);
        }
    }
}
