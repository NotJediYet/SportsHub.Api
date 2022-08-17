using SportsHub.Shared.Entities;
using Microsoft.AspNetCore.Http;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetTeamsAsync();
        
        Task<Team> GetTeamByIdAsync(Guid id);

        Task CreateTeamAsync(CreateTeamModel сreateTeamModel);

        Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName);

        Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id);
    }
}
