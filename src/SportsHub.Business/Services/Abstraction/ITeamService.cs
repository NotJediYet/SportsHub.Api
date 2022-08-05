using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetAllAsync();
        
        Task<Team> GetByIdAsync(Guid id);
        
        Task CreateAsync(string teamName, Guid subcategoryId);
        
        Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName);
    }
}
