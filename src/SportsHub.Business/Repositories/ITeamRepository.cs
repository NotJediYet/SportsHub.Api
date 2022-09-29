using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Repositories
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetTeamsAsync();

        Task<Team> GetTeamByIdAsync(Guid id);

        Task<Team> GetTeamByNameAsync(string teamName);

        Task AddTeamAsync(Team team);

        Task<Team> DeleteTeamAsync(Guid id);

        Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id);

        Task<Guid> FindTeamIdBySubcategoryIdAsync(Guid subcategoryId);

        Task EditTeamAsync(Team team);

        IEnumerable<Team> GetTeamsFilteredBySubcategoryId(Guid subcategoryId, ICollection<Team> teams);

        IEnumerable<Team> GetTeamsFilteredBySubcategoryIds(IEnumerable<Subcategory> subcategories, ICollection<Team> teams);
    }
}
