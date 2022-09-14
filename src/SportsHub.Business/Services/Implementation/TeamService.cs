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
            var teams = await _teamRepository.GetTeamsAsync();
            var teamLogos = await _teamLogoRepository.GetTeamLogosAsync();

            foreach (var team in teams)
            {
                var logo = teamLogos.FirstOrDefault(logo => logo.TeamId == team.Id);

                team.TeamLogo = ConvertTeamLogo(logo);
            }

            return teams;
        }

        public async Task<Team> GetTeamByIdAsync(Guid id)
        {
            var team = await _teamRepository.GetTeamByIdAsync(id);
            var teamLogo = await _teamLogoRepository.GetTeamLogoByTeamIdAsync(id);

            team.TeamLogo = ConvertTeamLogo(teamLogo);

            return team;
        }

        public async Task CreateTeamAsync(CreateTeamModel сreateTeamModel)
        {
            var newTeam = new Team { Name = сreateTeamModel.Name, 
                SubcategoryId = сreateTeamModel.SubcategoryId, 
                Location = сreateTeamModel.Location 
            };

            await _teamRepository.AddTeamAsync(newTeam);

            if (сreateTeamModel.TeamLogo != null)
            {
                var fileBytes = сreateTeamModel.TeamLogo.ToByteArray();
                var fileExtension = Path.GetExtension(сreateTeamModel.TeamLogo.FileName);
                var newTeamLogo = new TeamLogo(fileBytes, fileExtension, newTeam.Id);

                await _teamLogoRepository.AddTeamLogoAsync(newTeamLogo);
            }
        }

        public async Task<Guid> GetTeamIdByNameAsync(string teamName)
        {
            var team = await _teamRepository.GetTeamByNameAsync(teamName);

            if (team == null)
            {
                return Guid.Empty;
            }
            else
            {
                return team.Id;
            }
        }
        
        public async Task EditTeamAsync(EditTeamModel editTeamModel)
        {
            var teamModel = new Team
            {
                Id = editTeamModel.Id,
                Name = editTeamModel.Name,
                SubcategoryId = editTeamModel.SubcategoryId,
                Location = editTeamModel.Location,
                IsHidden = editTeamModel.IsHidden,
                OrderIndex = editTeamModel.OrderIndex
            };

            await _teamRepository.EditTeamAsync(teamModel);

            if (editTeamModel.TeamLogo != null) {
                var fileBytes = editTeamModel.TeamLogo.ToByteArray();
                var fileExtension = Path.GetExtension(editTeamModel.TeamLogo.FileName);
                var teamLogo = new TeamLogo(fileBytes, fileExtension, teamModel.Id);

                await _teamLogoRepository.EditTeamLogoAsync(teamLogo);
            }
        }

        public async Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id)
        {
            var result = await _teamRepository.DoesTeamAlreadyExistByIdAsync(id);

            return result;
        }

        public async Task<Guid> FindTeamIdBySubcategoryIdAsync(Guid subcategoryId)
        {
            return await _teamRepository.FindTeamIdBySubcategoryIdAsync(subcategoryId);
        }

        public IFormFile ConvertTeamLogo(TeamLogo logo)
        {
            if (logo != null)
            {
                var fileStream = new MemoryStream(logo.Bytes);

                IFormFile newFile = new FormFile(fileStream, 0, fileStream.Length, logo.TeamId.ToString(), logo.TeamId.ToString() + logo.FileExtension)
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "teamLogo/" + logo.FileExtension.TrimStart('.'),
                };

                return newFile;
            }

            else return null;
        }
    }
}
