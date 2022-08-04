using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services.Abstraction
{
    public interface ITeamService
    {
        Task<List<Team>> GetAllAsync();
        
        Task<Team> GetByIdAsync(Guid id);
        
        Task CreateAsync(string newName, Guid subcategoryId);
        
        Task<bool> CheckIfNameNotUniqueAsync(string newName);
        
        Task<bool> CheckIfSubcategoryIdNotExists(Guid id);
    }
}
