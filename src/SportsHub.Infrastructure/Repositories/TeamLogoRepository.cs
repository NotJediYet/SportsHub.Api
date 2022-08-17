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

        public async Task AddTeamLogoAsync(IFormFile teamLogoFile, Guid teamId)
        {
            var memoryStream = new MemoryStream();
            await teamLogoFile.CopyToAsync(memoryStream);

            await teamLogoFile.CopyToAsync(memoryStream);

            var fileBytes = memoryStream.ToArray();
            var fileExtension = Path.GetExtension(teamLogoFile.FileName);
            TeamLogo newTeamLogo = new TeamLogo(fileBytes, fileExtension, teamId);

            await _context.Set<TeamLogo>().AddAsync(newTeamLogo);

            await _context.Set<TeamLogo>().AddAsync(newTeamLogo);
            await _context.Set<TeamLogo>().AddAsync(newTeamLogo);
            await _context.SaveChangesAsync();
        }
    }
}
