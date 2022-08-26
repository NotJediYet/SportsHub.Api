using SportsHub.Business.Repositories;
using SportsHub.Shared.Entities;
using Microsoft.AspNetCore.Http;
using SportsHub.Shared.Models;
using SportsHub.Extensions;

namespace SportsHub.Business.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly ITeamLogoRepository _teamLogoRepository;

        public TeamService(ITeamRepository teamRepository, ITeamLogoRepository teamLogoRepository)
        {
            _teamRepository = teamRepository ?? throw new ArgumentNullException(nameof(teamRepository));
            _teamLogoRepository = teamLogoRepository ?? throw new ArgumentNullException(nameof(teamLogoRepository));
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

        public async Task CreateTeamAsync(CreateTeamModel сreateTeamModel)
        {
            var newTeam = new Team()
            {
                Name = сreateTeamModel.Name, 
                SubcategoryId = сreateTeamModel.SubcategoryId, 
                Location = сreateTeamModel.Location
            };
            await _teamRepository.AddTeamAsync(newTeam);

            using var memoryStream = new MemoryStream();
            await сreateTeamModel.Logo.CopyToAsync(memoryStream);

            var fileBytes = сreateTeamModel.Logo.ByteArray();
            var fileExtension = Path.GetExtension(сreateTeamModel.Logo.FileName);
            var newTeamLogo = new TeamLogo(fileBytes, fileExtension, newTeam.Id);

            await _teamLogoRepository.AddTeamLogoAsync(newTeamLogo);
        }

        public async Task<Guid> DoesTeamAlreadyExistByNameAsync(string teamName)
        {
            var result = await _teamRepository.DoesTeamAlreadyExistByNameAsync(teamName);

            return result;
        }

        public async Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id)
        {
            var result = await _teamRepository.DoesTeamAlreadyExistByIdAsync(id);

            return result;
        }
        
        public async Task EditTeamAsync(Team team)
        {
            await _teamRepository.EditTeamAsync(team);

            using var memoryStream = new MemoryStream();
            await team.TeamLogo.CopyToAsync(memoryStream);

            var fileBytes = team.TeamLogo.ByteArray();
            var fileExtension = Path.GetExtension(team.TeamLogo.FileName);
            var teamLogo = new TeamLogo(fileBytes, fileExtension, team.Id);

            await _teamLogoRepository.EditTeamLogoAsync(teamLogo);
        }
    }
}
