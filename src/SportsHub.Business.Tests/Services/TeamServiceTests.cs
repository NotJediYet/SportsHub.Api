using Moq;
using SportsHub.Business.Repositories;
using SportsHub.Business.Services;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace SportsHub.Business.Tests.Services
{
    public class TeamServiceTests
    {
        private readonly Mock<ITeamRepository> _teamRepository;
        private readonly Mock<ITeamLogoRepository> _teamLogoRepository;
        private readonly ITeamService _service;

        public TeamServiceTests()
        {
            _teamRepository = new Mock<ITeamRepository>();
            _teamLogoRepository = new Mock<ITeamLogoRepository>();
            _service = new TeamService(_teamRepository.Object, _teamLogoRepository.Object);
        }

        [Fact]
        public async Task GetTeamsAsync_ReturnsExpectedTeams()
        {
            // Arrange
            var expectedTeams = GetTeams();

            _teamRepository.Setup(repository => repository.GetTeamsAsync())
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

            var expectedTeam = new Team(name: "Name", Guid.NewGuid(), location: "Location");
            expectedTeam.Id = expectedTeamId;

            _teamRepository.Setup(repo => repo.GetTeamByIdAsync(expectedTeamId))
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
            var expectedLocation = "Location";

            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            var expectedTeamLogo = fileMock.Object;

            CreateTeamModel сreateTeamModel = new()
            {
                Name = expectedTeamName,
                Location = expectedLocation,
                SubcategoryId = expectedSubcategoryId,
                Logo = expectedTeamLogo
            };      

            // Act
            await _service.CreateTeamAsync(сreateTeamModel);

            // Assert
            _teamRepository.Verify(repository => repository.AddTeamAsync(It.Is<Team>(team =>
                (team.Name == expectedTeamName) && (team.SubcategoryId == expectedSubcategoryId))));
        }
        [Fact]
        public async Task DoesTeamAlreadyExistByNameAsync_WhenTeamExists_ReturnsTrue()
        {
            // Arrange
            var teamName = "Name";

            _teamRepository.Setup(repository => repository.DoesTeamAlreadyExistByNameAsync(teamName))
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

            _teamRepository.Setup(repository => repository.DoesTeamAlreadyExistByNameAsync(teamName))
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
                new Team("Name1", Guid.NewGuid(), "Location1"),
                new Team("Name2", Guid.NewGuid(), "Location2"),
                new Team("Name3", Guid.NewGuid(), "Location3")
            };

            return teams;
        }
    }
}
