using SportsHub.Shared.Entities;
using Microsoft.AspNetCore.Http;

namespace SportsHub.Business.Repositories
{
    public interface ITeamLogoRepository {
        Task<TeamLogo> GetTeamLogoByTeamIdAsync(Guid teamId);

        Task AddTeamLogoAsync(IFormFile teamLogoFile, Guid teamId);

        Task EditTeamLogoAsync(IFormFile teamLogoFile, Guid teamId);
    }
}