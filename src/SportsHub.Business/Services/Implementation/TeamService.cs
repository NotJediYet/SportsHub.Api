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
    }
}
