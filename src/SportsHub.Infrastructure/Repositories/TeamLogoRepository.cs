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

    public async Task<bool> DoesTeamLogoAlreadyExistByTeamIdAsync(Guid teamId)
        {
            var teamLogos = await _context.Set<TeamLogo>().ToListAsync();

            return teamLogos.Any(teamLogo => teamLogo.TeamId == teamId);
        }

        public async Task EditTeamLogoAsync(TeamLogo teamLogo)
        {
            _context.TeamLogos.Update(teamLogo);

            await _context.SaveChangesAsync();
        }

        public async Task EditTeamLogoAsync(IFormFile teamLogoFile, Guid teamId)
        {
            var logo = await _context.TeamLogos.Where(logo => logo.TeamId == teamId).FirstOrDefaultAsync();

            var memoryStream = new MemoryStream();
            await teamLogoFile.CopyToAsync(memoryStream);

            logo.Bytes = memoryStream.ToArray();
            logo.FileExtension = Path.GetExtension(teamLogoFile.FileName);

            await _context.SaveChangesAsync();
        }
    }
}
