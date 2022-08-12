﻿using SportsHub.Infrastructure.DBContext;
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
    }
}