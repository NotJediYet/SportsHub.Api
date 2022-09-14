using SportsHub.Shared.Entities;

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

        IEnumerable<Team> GetTeamsFilteredByLocation(string location, ICollection<Team> teams);

        IEnumerable<Team> GetTeamsFilteredBySubcategoryId(Guid subcategoryId, ICollection<Team> teams);

        IEnumerable<Team> GetTeamsFilteredBySubcategoryIds(IEnumerable<Subcategory> subcategories, ICollection<Team> teams);

        Task<IEnumerable<Team>> GetSortedTeamAsync();
    }
}
