using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;
using Microsoft.AspNetCore.Http;

namespace SportsHub.Business.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetTeamsAsync();
        
        Task<Team> GetTeamByIdAsync(Guid id);

        Task CreateTeamAsync(CreateTeamModel сreateTeamModel);

        Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName);
       
        Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id);

        Task<Guid> FindTeamIdByTeamNameAsync(string teamName);

        Task<Guid> FindTeamIdBySubcategoryIdAsync(Guid subcategoryId);
    }
}
