using SportsHub.Shared.Entities;
using Microsoft.AspNetCore.Http;

namespace SportsHub.Business.Repositories
{
    public interface ITeamLogoRepository {
        Task<TeamLogo> GetTeamLogoByTeamIdAsync(Guid teamId);

        Task AddTeamLogoAsync(TeamLogo teamLogo);

        Task AddTeamLogoAsync(IFormFile teamLogoFile, Guid teamId);

        Task<bool> DoesTeamLogoAlreadyExistByTeamIdAsync(Guid teamId);

        Task EditTeamLogoAsync(TeamLogo teamLogo);
    }
}