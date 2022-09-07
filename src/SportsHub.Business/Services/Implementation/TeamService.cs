using SportsHub.Business.Repositories;
using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepositor)
        {
            _teamRepository = teamRepositor ?? throw new ArgumentNullException(nameof(teamRepositor));
        }
       
        public async Task<IEnumerable<Team>> GetTeamsAsync()
        {
            return await _teamRepository.GetTeamsAsync();
        }

        public async Task<Team> GetTeamByIdAsync(Guid id)
        {
            return await _teamRepository.GetTeamByIdAsync(id);
        }

        public async Task CreateTeamAsync(string teamName, Guid subcategoryId)
        {
            await _teamRepository.AddTeamAsync(new Team(teamName, subcategoryId));
        }

        public async Task<Team> DeleteTeamAsync(Guid id)
        {
            return await _teamRepository.DeleteTeamAsync(id);
        }

        public async Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName)
        {
            return await _teamRepository.DoesTeamAlreadyExistByNameAsync(teamName);
        }

        public async Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id)
        {
            return await _teamRepository.DoesTeamAlreadyExistByIdAsync(id);
        }

        public async Task<Guid> FindTeamIdByTeamNameAsync(string teamName)
        {
            return await _teamRepository.FindTeamIdByTeamNameAsync(teamName);
        }

        public async Task<Guid> FindTeamIdBySubcategoryIdAsync(Guid subcategoryId)
        {
            return await _teamRepository.FindTeamIdBySubcategoryIdAsync(subcategoryId);
        }
    }
}
