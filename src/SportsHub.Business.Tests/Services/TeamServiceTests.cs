using Moq;
using SportsHub.Business.Repositories;
using SportsHub.Business.Services;
using SportsHub.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SportsHub.Business.Tests.Services
{
    public class TeamServiceTests
    {
        private readonly Mock<ITeamRepository> _repository;
        private readonly ITeamService _service;

        public TeamServiceTests()
        {
            _repository = new Mock<ITeamRepository>();
            _service = new TeamService(_repository.Object);
        }

        [Fact]
        public async Task GetTeamsAsync_ReturnsExpectedTeams()
        {
            // Arrange
            var expectedTeams = GetTeams();

            _repository.Setup(repository => repository.GetTeamsAsync())
                .ReturnsAsync(expectedTeams);

            // Act
            var actualTeams = await _service.GetTeamsAsync();

            // Assert
            Assert.Equal(expectedTeams, actualTeams);
        }

        [Fact]
        public async Task GetTeamByIdAsync_WhenIdIsValid_ReturnsExpectedTeam()
        {
            // Arrange
            var expectedTeamId = Guid.NewGuid();

            var expectedTeam = new Team(name: "Name", Guid.NewGuid());
            expectedTeam.Id = expectedTeamId;

            _repository.Setup(repo => repo.GetTeamByIdAsync(expectedTeamId))
                .ReturnsAsync(expectedTeam);

            // Act
            var actualTeam = await _service.GetTeamByIdAsync(expectedTeamId);

            // Assert
            Assert.Equal(expectedTeam.Name, actualTeam.Name);
            Assert.Equal(expectedTeam.Id, actualTeam.Id);
            Assert.Equal(expectedTeam.SubcategoryId, actualTeam.SubcategoryId);
        }

        [Fact]
        public async Task CreateTeamAsync_CallsAppropriateRepositoryMethodWithParameters()
        {
            // Arrange
            var expectedTeamName = "Name";
            var expectedSubcategoryId = Guid.NewGuid();

            // Act
            await _service.CreateTeamAsync(expectedTeamName, expectedSubcategoryId);

            // Assert
            _repository.Verify(repository => repository.AddTeamAsync(It.Is<Team>(team =>
                (team.Name == expectedTeamName) && (team.SubcategoryId == expectedSubcategoryId))));
        }

        [Fact]
        public async Task DoesTeamAlreadyExistByNameAsync_WhenTeamExists_ReturnsTrue()
        {
            // Arrange
            var teamName = "Name";

            _repository.Setup(repository => repository.DoesTeamAlreadyExistByNameAsync(teamName))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DoesTeamAlreadyExistByNameAsync(teamName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesTeamAlreadyExistByNameAsync_WhenTeamDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var teamName = "Name";

            _repository.Setup(repository => repository.DoesTeamAlreadyExistByNameAsync(teamName))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DoesTeamAlreadyExistByNameAsync(teamName);

            // Assert
            Assert.False(result);
        }

        private IEnumerable<Team> GetTeams()
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
