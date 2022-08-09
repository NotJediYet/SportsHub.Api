using SportsHub.Shared.Entities;

namespace SportsHub.Repositories
{
    public interface ITeamRepository
    {
        Task<IEnumerable<Team>> GetTeamsAsync();

        Task<Team> GetTeamByIdAsync(Guid id);

        Task AddTeamAsync(Team team);

        Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName);
    }
}
