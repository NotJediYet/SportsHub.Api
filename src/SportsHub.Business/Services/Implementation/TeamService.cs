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
            return await _teamRepository.GetAllAsync();
        }

        public async Task<Team> GetTeamByIdAsync(Guid id)
        {
            var team = await _teamRepository.GetByIdAsync(id);

            return team;
        }

        public async Task CreateTeamAsync(string teamName, Guid subcategoryId)
        {
            await _teamRepository.AddAsync(new Team(teamName, subcategoryId));

            await _teamRepository.SaveAsync();
        }

        public async Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName)
        {
            var teams = await _teamRepository.GetAllAsync();

            return teams.Any(team => team.Name == teamName);
        }
    }
}
