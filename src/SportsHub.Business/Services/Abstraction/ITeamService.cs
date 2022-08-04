using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services.Abstraction
{
    public interface ITeamService
    {
        Task<List<Team>> GetAllAsync();
        
        Task<Team> GetByIdAsync(Guid id);
        
        Task CreateAsync(string teamName, Guid subcategoryId);
        
        Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName);
        
        Task<bool> DoesTeamAlredyExistByIdAsync(Guid id);
    }
}
