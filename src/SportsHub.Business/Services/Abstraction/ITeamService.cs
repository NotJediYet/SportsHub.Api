using SportsHub.Shared.Entities;
using Microsoft.AspNetCore.Http;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetTeamsAsync();
        
        Task<Team> GetTeamByIdAsync(Guid id);

        Task CreateTeamAsync(CreateTeamModel сreateTeamModel);

        Task<Guid> GetTeamIdByNameAsync(string teamName);

        Task<Team> DeleteTeamAsync(Guid id);

        Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id);

        Task<Guid> FindTeamIdBySubcategoryIdAsync(Guid subcategoryId);

        Task EditTeamAsync(EditTeamModel editTeamModel);

        IEnumerable<Team> GetTeamsFilteredBySubcategoryId(Guid subcategoryId, ICollection<Team> teams);

        IEnumerable<Team> GetTeamsFilteredBySubcategoryIds(IEnumerable<Subcategory> subcategories, ICollection<Team> teams);
    }
}
