using SportsHub.Business.Repositories;
using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    internal class TeamService : ITeamService
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
            var team = await _teamRepository.GetTeamByIdAsync(id);

            return team;
        }

        public async Task CreateTeamAsync(string teamName, Guid subcategoryId)
        {
            await _teamRepository.AddTeamAsync(new Team(teamName, subcategoryId));
        }

        public async Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName)
        {
            var result = await _teamRepository.DoesTeamAlreadyExistByNameAsync(teamName);

            return result;
        }

        public async Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id)
        {
            var result = await _teamRepository.DoesTeamAlreadyExistByIdAsync(id);

            return result;
        }

        public async Task<Guid> FindTeamIdByTeamName(string teamName)
        {
            return await _teamRepository.FindTeamIdByTeamName(teamName);
        }

        public async Task<Guid> FindTeamIdBySubcategoryId(Guid subcategoryId)
        {
            return await _teamRepository.FindTeamIdBySubcategoryId(subcategoryId);
        }
    }
}
