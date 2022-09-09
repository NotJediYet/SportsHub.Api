﻿using SportsHub.Business.Repositories;
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

        public TeamService(ITeamRepository teamRepository, 
                           ITeamLogoRepository teamLogoRepository)
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
            return await _teamRepository.GetTeamByIdAsync(id);
        }

        public async Task CreateTeamAsync(CreateTeamModel сreateTeamModel)
        {
            var newTeam = new Team(сreateTeamModel.Name, сreateTeamModel.SubcategoryId, сreateTeamModel.Location);
            await _teamRepository.AddTeamAsync(newTeam);

            if (сreateTeamModel.Logo != null)
            {
                var fileBytes = сreateTeamModel.Logo.ToByteArray();
                var fileExtension = Path.GetExtension(сreateTeamModel.Logo.FileName);
                var newTeamLogo = new TeamLogo(fileBytes, fileExtension, newTeam.Id);

                await _teamLogoRepository.AddTeamLogoAsync(newTeamLogo);
            }
        }

        public async Task<bool> DoesTeamAlreadyExistByNameAsync(string teamName)
        {
            return await _teamRepository.DoesTeamAlreadyExistByNameAsync(teamName);
        }

        public async Task<Guid> FindTeamIdByTeamNameAsync(string teamName)
        {
            return await _teamRepository.FindTeamIdByTeamNameAsync(teamName);
        }

        public async Task<Guid> FindTeamIdBySubcategoryIdAsync(Guid subcategoryId)
        {
            return await _teamRepository.FindTeamIdBySubcategoryIdAsync(subcategoryId);
        }

        public async Task<bool> DoesTeamAlreadyExistByIdAsync(Guid id)
        {
            var result = await _teamRepository.DoesTeamAlreadyExistByIdAsync(id);

            return result;
        }
    }
}
