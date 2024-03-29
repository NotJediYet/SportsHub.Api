﻿using SportsHub.Infrastructure.DBContext;
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
            return await _context.TeamLogos.FindAsync(teamId);
        }

        public async Task<IEnumerable<TeamLogo>> GetTeamLogosAsync()
        {
            return await _context.TeamLogos.ToListAsync();
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
            var existingTeamLogo = await _context.TeamLogos.FirstOrDefaultAsync(oldTeamLogo => oldTeamLogo.TeamId == teamLogo.TeamId);

            if (existingTeamLogo == null)
                await _context.Set<TeamLogo>().AddAsync(teamLogo);
            else
            {
                existingTeamLogo.Bytes = teamLogo.Bytes;
                existingTeamLogo.FileExtension = teamLogo.FileExtension;
            }

            await _context.SaveChangesAsync();
        }
    }
}
