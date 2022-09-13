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

        Task<Guid> GetTeamIdByNameAsync(string teamName);

        Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id);

        Task<Guid> FindTeamIdBySubcategoryIdAsync(Guid subcategoryId);

        Task EditTeamAsync(EditTeamModel editTeamModel);

        List<Team> GetTeamsFilteredByLocation(string location, List<Team> teams);

        List<Team> GetTeamsFilteredBySubcategoryIds(List<Guid> subcategoryIds, List<Team> teams);

        List<Team> GetTeamsFilteredBySubcategoryId(Guid subcategoryId, List<Team> teams);

        Task<List<Team>> GetSortedTeamAsync();
    }
}
