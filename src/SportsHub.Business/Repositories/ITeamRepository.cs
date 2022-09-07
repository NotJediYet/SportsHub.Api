using SportsHub.Shared.Entities;

namespace SportsHub.Business.Repositories
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetTeamsAsync();

        Task<Team> GetTeamByIdAsync(Guid id);

        Task AddTeamAsync(Team team);

        Task<Team> DeleteTeamAsync(Guid id);

        Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName);
        
        Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id);

        Task<Guid> FindTeamIdByTeamNameAsync(string teamName);

        Task<Guid> FindTeamIdBySubcategoryIdAsync(Guid subcategoryId);
    }
}
