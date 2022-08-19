﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<Guid> DoesTeamAlreadyExistByNameAsync(string teamName)
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

        public async Task UpdateTeamAsync(EditTeamModel Team)
        {
            var team = await _context.Teams.Where(team => team.Id == Team.Id).FirstOrDefaultAsync();

            team.Name = Team.Name;
            team.Location = Team.Location;
            team.SubcategoryId = Team.SubcategoryId;

            await _context.SaveChangesAsync();

        }
    }
}