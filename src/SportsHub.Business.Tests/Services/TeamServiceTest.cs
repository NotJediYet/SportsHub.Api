using Moq;
using SportsHub.Business.Repositories;
using SportsHub.Business.Services;
using SportsHub.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SportsHub.Business.Tests.Services
{
    public class TeamServiceTest
    {
        private readonly Mock<ITeamRepository> _repository;
        private readonly ITeamService _service;

        public TeamServiceTest()
        {
            _repository = new Mock<ITeamRepository>();
            _service = new TeamService(_repository.Object);
        }
            
        [Fact]
        public async Task GetTeamsAsync_ReturnsCollectionOfTeams()
        {
            // Arrange
            _repository.Setup(repo => repo.GetTeamsAsync())
                .ReturnsAsync(GetTestTeams());

            // Act
            var result = await _service.GetTeamsAsync();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Team>>(result);
            Assert.Equal(GetTestTeams().Count(), result.Count());
        }
            
        [Fact]
        public async Task GetTeamByIdAsync_HasValidId_ReturnsTeam()
        {
            // Arrange
            var testTeam = GetTestTeams().FirstOrDefault();

            _repository.Setup(repo => repo.GetTeamByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(testTeam);

            // Act
            var result = await _service.GetTeamByIdAsync(Guid.NewGuid());

            // Assert
            var team = Assert.IsType<Team>(result);
            Assert.Equal(testTeam.Name, team.Name);
            Assert.Equal(testTeam.SubcategoryId, team.SubcategoryId);
            Assert.Equal(testTeam.Id, team.Id);
        }

        [Fact]
        public async Task CreateTeamAsync_HasValidValues()
        {
            // Arrange
            var teamName = "Name";

            // Act
            await _service.CreateTeamAsync(teamName, Guid.NewGuid());

            // Assert
            _repository.Verify(repo => repo.AddTeamAsync(It.IsAny<Team>()));
        }

        [Fact]
        public async Task DoesTeamAlreadyExistByNameAsync_HasExistedName_ReturnsTrue()
        {
            // Arrange
            var teamName = GetTestTeams().FirstOrDefault().Name;

            _repository.Setup(repo => repo.DoesTeamAlreadyExistByNameAsync(teamName))
                .ReturnsAsync(GetTestTeams().Any(team => team.Name == teamName));

            // Act
            var result = await _service.DoesTeamAlreadyExistByNameAsync(teamName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesTeamAlreadyExistByNameAsync_HasNotExistedName_ReturnsFalse()
        {
            // Arrange
            var teamName = "Test unique name";

            _repository.Setup(repo => repo.DoesTeamAlreadyExistByNameAsync(teamName))
                .ReturnsAsync(GetTestTeams().Any(team => team.Name == teamName));

            // Act
            var result = await _service.DoesTeamAlreadyExistByNameAsync(teamName);

            // Assert
            Assert.False(result);
        }

        private IEnumerable<Team> GetTestTeams()
        {
            IEnumerable<Team> teams = new List<Team>
            {
                new Team("Name1", Guid.NewGuid()),
                new Team("Name2", Guid.NewGuid()),
                new Team("Name3", Guid.NewGuid())
            };
            return teams;
        }
    }
}
