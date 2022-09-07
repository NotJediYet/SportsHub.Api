using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetTeamsAsync();
        
        Task<Team> GetTeamByIdAsync(Guid id);
        
        Task CreateTeamAsync(string teamName, Guid subcategoryId);

        Task<Team> DeleteTeamAsync(Guid Id);
        
        Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName);
       
        Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id);
        
        Task<Guid> FindTeamIdByTeamNameAsync(string teamName);

        Task<Guid> FindTeamIdBySubcategoryIdAsync(Guid subcategoryId);
    }
}
