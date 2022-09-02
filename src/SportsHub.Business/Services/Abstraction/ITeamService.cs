using SportsHub.Shared.Entities;
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

        Task<Guid> FindTeamIdByTeamNameAsync(string teamName);

        Task<Guid> FindTeamIdBySubcategoryIdAsync(Guid subcategoryId);

        List<Team> GetTeamsFilteredByLocation(string location, List<Team> teams);

        List<Team> GetTeamsFilteredBySubcategoryIds(List<Guid> subcategoryIds, List<Team> teams);

        List<Team> GetTeamsFilteredBySubcategoryId(Guid subcategoryId, List<Team> teams);

        Task<List<Team>> GetSortedTeamAsync();
    }
}
