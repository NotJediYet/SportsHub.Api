using SportsHub.Shared.Entities;
using Microsoft.AspNetCore.Http;

namespace SportsHub.Business.Repositories
{
    public interface ITeamLogoRepository {
        Task<IEnumerable<TeamLogo>> GetTeamLogosAsync();

        Task<TeamLogo> GetTeamLogoByIdAsync(Guid id);

        Task AddTeamLogoAsync(IFormFile teamLogoFile, Guid teamId);

        Task<bool> DoesTeamLogoAlreadyExistByTeamIdAsync(Guid teamId);

        Task<bool> DoesTeamLogoAlreadyExistByIdAsync(Guid id);
    }
}