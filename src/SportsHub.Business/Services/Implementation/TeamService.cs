using SportsHub.Business.Repositories;
using SportsHub.Shared.Entities;
using Microsoft.AspNetCore.Http;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamLogoRepository _teamLogoRepository;

        public TeamService(ITeamRepository teamRepositor, ITeamLogoRepository teamLogoRepositor)
        {
            _teamRepository = teamRepositor ?? throw new ArgumentNullException(nameof(teamRepositor));
            _teamLogoRepository = teamLogoRepositor ?? throw new ArgumentNullException(nameof(teamLogoRepositor));
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

        public async Task CreateTeamAsync(string teamName, Guid subcategoryId, string location, IFormFile teamLogo)
        {
            Team newTeam = new Team(teamName, subcategoryId, location);
            await _teamRepository.AddTeamAsync(newTeam);
            await _teamLogoRepository.AddTeamLogoAsync(teamLogo, newTeam.Id);
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
        
        public async Task EditTeam(EditTeamModel teamModel)
        {
            await _teamRepository.UpdateTeamAsync(teamModel);
        }
    }
}
