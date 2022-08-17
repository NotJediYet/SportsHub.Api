using SportsHub.Shared.Entities;
using Microsoft.AspNetCore.Http;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetTeamsAsync();
        
        Task<Team> GetTeamByIdAsync(Guid id);

        Task CreateTeamAsync(string teamName, Guid subcategoryId, string location, IFormFile teamLogo);

        Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName);

        Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id);
        
        Task EditTeam(EditTeamModel teamModel);
    }
}
