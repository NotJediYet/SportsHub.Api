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

        public async Task<TeamLogo> GetTeamLogoByTeamIdAsync(Guid teamId)
        {
            return await _context.Set<TeamLogo>().FindAsync(teamId);
        }

        public async Task AddTeamLogoAsync(TeamLogo teamLogo)
        {
            await _context.Set<TeamLogo>().AddAsync(teamLogo);

            await _context.SaveChangesAsync();
        }

        public byte[] GetTeamLogoByteArray(IFormFile fileLogo)
        {
            using var ms = new MemoryStream();
            fileLogo.CopyTo(ms);
            return ms.ToArray();    
        }

        public async Task<bool> DoesTeamLogoAlreadyExistByTeamIdAsync(Guid teamId)
        {
            var teamLogos = await _context.Set<TeamLogo>().ToListAsync();

            return teamLogos.Any(teamLogo => teamLogo.TeamId == teamId);
        }
    }
}
