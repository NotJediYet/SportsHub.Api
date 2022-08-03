using SportsHub.Shared.Models;

namespace SportsHub.Business.Services.Abstraction
{
    public interface ITeamService
    {
        Task<List<Team>> GetAllAsync();
        Task<Team?> GetByIDAsync(Guid Id);
        Task CreateAsync(Team Team);
        Task<bool> CheckIfNameNotUniqueAsync(string NewName);
        Task<bool> CheckIfIdNotUniqueAsync(Guid Id);
        Task<bool> CheckIfSubcategoryIdNotExists(Guid Id);
    }
}
