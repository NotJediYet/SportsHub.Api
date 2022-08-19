using SportsHub.Shared.Entities;
using Microsoft.AspNetCore.Http;

namespace SportsHub.Business.Repositories
{
    public interface ITeamLogoRepository {
        Task<TeamLogo> GetTeamLogoByTeamIdAsync(Guid teamId);

        Task AddTeamLogoAsync(TeamLogo teamLogo);

        byte[] GetTeamLogoByteArray(IFormFile fileLogo);

        Task<bool> DoesTeamLogoAlreadyExistByTeamIdAsync(Guid teamId);
    }
}