using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Repositories
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetTeamsAsync();

        Task<Team> GetTeamByIdAsync(Guid id);

        Task AddTeamAsync(Team team);

        Task<Guid> GetTeamIdByNameAsync(string teamName);

        Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id);

        Task<Guid> FindTeamIdBySubcategoryIdAsync(Guid subcategoryId);

        Task EditTeamAsync(Team team);

        List<Team> GetTeamsFilteredByLocation(string location, List<Team> teams);

        List<Team> GetTeamsFilteredBySubcategoryIds(List<Guid> subcategoryIds, List<Team> teams);

        List<Team> GetTeamsFilteredBySubcategoryId(Guid subcategoryId, List<Team> teams);

        Task<List<Team>> GetSortedTeamAsync();
    }
}
