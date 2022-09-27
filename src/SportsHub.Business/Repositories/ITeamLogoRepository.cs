using SportsHub.Shared.Entities;

namespace SportsHub.Business.Repositories
{
    public interface ITeamLogoRepository {
        Task<TeamLogo> GetTeamLogoByTeamIdAsync(Guid teamId);

        Task<IEnumerable<TeamLogo>> GetTeamLogosAsync();

        Task AddTeamLogoAsync(TeamLogo teamLogo);

        Task<bool> DoesTeamLogoAlreadyExistByTeamIdAsync(Guid teamId);

        Task EditTeamLogoAsync(TeamLogo teamLogo);
    }
}