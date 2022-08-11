﻿using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetTeamsAsync();
        
        Task<Team> GetTeamByIdAsync(Guid id);
        
        Task CreateTeamAsync(string teamName, Guid subcategoryId, string location);
        
        Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName);

        Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id);
    }
}
