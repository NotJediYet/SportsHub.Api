using Moq;
using SportsHub.Business.Repositories;
using SportsHub.Business.Services;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;

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

            var expectedTeam = new Team()
            {
                Name = "Name",
                SubcategoryId = Guid.NewGuid(),
                Location = "location"
            };
            expectedTeam.Id = expectedTeamId;

            _teamRepository.Setup(repository => repository.GetTeamByIdAsync(expectedTeamId))
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
            var byteArray = Encoding.UTF8.GetBytes("This is a dummy file");
            var expectedTeamLogo = new FormFile(new MemoryStream(byteArray), 0, byteArray.Length, "Data", "image.jpg");
            var expectedTeamLogoExtension = Path.GetExtension(expectedTeamLogo.FileName);

            var expectedTeamName = "Name";
            var expectedSubcategoryId = Guid.NewGuid();
            var expectedTeamId = Guid.NewGuid();
            var teamId = expectedTeamId;
            var expectedLocation = "Location";
            var expectedByteArray = byteArray;
            var fileExtension = expectedTeamLogoExtension;

            CreateTeamModel сreateTeamModel = new()
            {
                Name = expectedTeamName,
                Location = expectedLocation,
                SubcategoryId = expectedSubcategoryId,
                TeamLogo = expectedTeamLogo
            };

            // Act
            await _service.CreateTeamAsync(сreateTeamModel);

            // Assert
            _teamRepository.Verify(repository => repository.AddTeamAsync(It.Is<Team>(team =>
                (team.Name == expectedTeamName) && (team.SubcategoryId == expectedSubcategoryId))));

            _teamLogoRepository.Verify(repository => repository.AddTeamLogoAsync(It.Is<TeamLogo>(teamLogo =>
                (byteArray == expectedByteArray)
                && (fileExtension == expectedTeamLogoExtension)
                && (teamId == expectedTeamId)
                )));
        }
        [Fact]
        public async Task GetTeamByNameAsync_WhenTeamExists_ReturnsTeamId()
        {
            // Arrange
            var expectedId = Guid.NewGuid();

            var team = new Team()
            {
                Id = expectedId,
                Name = "Name1",
                SubcategoryId = Guid.NewGuid(),
                Location = "Location1"
            };

            _teamRepository.Setup(repository => repository.GetTeamByNameAsync(team.Name))
            .ReturnsAsync(team);

            // Act
            var actualId = await _service.GetTeamIdByNameAsync(team.Name);

            // Assert
            Assert.Equal(expectedId, actualId);
        }

        [Fact]
        public async Task GetTeamByNameAsync_WhenTeamDoesNotExists_ReturnsEmptyId()
        {
            // Arrange
            var teamName = "Name";
            Team team = null;

            _teamRepository.Setup(repository => repository.GetTeamByNameAsync(teamName))
            .ReturnsAsync(team);

            // Act
            var actualId = await _service.GetTeamIdByNameAsync(teamName);

            // Assert
            Assert.Equal(Guid.Empty, actualId);
        }

        [Fact]
        public async Task FindTeamIdByTeamNameAsync_ReturnsTeamId()
        {
            // Arrange
            var expectedTeamId = Guid.NewGuid();

            var team = new Team()
            {
                Id = expectedTeamId,
                Name = "Name1",
                SubcategoryId = Guid.NewGuid(),
                Location = "Location1"
            };

            _teamRepository.Setup(repository => repository.GetTeamByNameAsync(team.Name))
                .ReturnsAsync(team);

            // Act
            var actualTeamId = await _service.GetTeamIdByNameAsync(team.Name);

            // Assert
            Assert.Equal(expectedTeamId, actualTeamId);
        }

        [Fact]
        public async Task FindTeamIdBySubcategoryIdAsync_ReturnsTeamId()
        {
            // Arrange
            var expectedTeamId = Guid.NewGuid();
            var expectedSubcategoryId = Guid.NewGuid();

            _teamRepository.Setup(repository => repository.FindTeamIdBySubcategoryIdAsync(expectedSubcategoryId))
                .ReturnsAsync(expectedTeamId);

            // Act
            var actualTeamId = await _service.FindTeamIdBySubcategoryIdAsync(expectedSubcategoryId);

            // Assert
            Assert.Equal(expectedTeamId, actualTeamId);
        }

        [Fact]
        public void GetTeamsFilteredByLocation_ReturnsExpectedTeams()
        {
            // Arrange
            var subcategoryId = Guid.NewGuid();
            var expectedTeams = GetTeams().ToList();
            var expectedLocation = "USA";

            _teamRepository.Setup(repository => repository.GetTeamsFilteredByLocation(expectedLocation, expectedTeams))
                .Returns(expectedTeams);

            // Act
            var actualTeams = _service.GetTeamsFilteredByLocation(expectedLocation, expectedTeams);

            // Assert
            Assert.Equal(expectedTeams, actualTeams);
        }

        [Fact]
        public void GetTeamsFilteredBySubcategoryIds_ReturnsExpectedTeams()
        {
            // Arrange
            var subcategoryId = Guid.NewGuid();
            ICollection<Subcategory> subcategories = new List<Subcategory>
            {
                new Subcategory { Name = "Name", CategoryId = Guid.NewGuid() },
                new Subcategory { Name = "Name", CategoryId = Guid.NewGuid() },
                new Subcategory { Name = "Name", CategoryId = Guid.NewGuid() }
    };
            var expectedTeams = GetTeams().ToList();

            _teamRepository.Setup(repository => repository.GetTeamsFilteredBySubcategoryIds(subcategories, expectedTeams))
                .Returns(expectedTeams);

            // Act
            var actualTeams = _service.GetTeamsFilteredBySubcategoryIds(subcategories, expectedTeams);

            // Assert
            Assert.Equal(expectedTeams, actualTeams);
        }

        [Fact]
        public void GetTeamsFilteredBySubcategoryId_ReturnsExpectedTeams()
        {
            // Arrange
            var subcategoryId = Guid.NewGuid();
            var expectedTeams = GetTeams().ToList();

            _teamRepository.Setup(repository => repository.GetTeamsFilteredBySubcategoryId(subcategoryId, expectedTeams))
                .Returns(expectedTeams);

            // Act
            var actualTeams = _service.GetTeamsFilteredBySubcategoryId(subcategoryId, expectedTeams);

            // Assert
            Assert.Equal(expectedTeams, actualTeams);
        }

        [Fact]
        public async Task GetSortedTeamAsync_ReturnsSortedTeams()
        {
            // Arrange
            var expectedTeams = GetTeams();

            _teamRepository.Setup(repository => repository.GetSortedTeamAsync())
                .Returns(Task.FromResult(expectedTeams));

            // Act
            var actualTeams = await _service.GetSortedTeamAsync();

            // Assert
            Assert.Equal(expectedTeams, actualTeams);
        }


        private IEnumerable<Team> GetTeams()
        {
            IEnumerable<Team> teams = new List<Team>
            {
                new Team()
                {
                    Name = "Name1",
                    SubcategoryId = Guid.NewGuid(),
                    Location = "Location1"
                },
                new Team()
                {
                    Name = "Name2",
                    SubcategoryId = Guid.NewGuid(),
                    Location = "Location2"
                },
                new Team()
                {
                    Name = "Name3",
                    SubcategoryId = Guid.NewGuid(),
                    Location = "Location3"
                }
            };

            return teams;
        }

        [Fact]
        public async Task EditTeamAsync_CallsAppropriateRepositoryMethodWithParameters()
        {
            // Arrange
            var byteArray = Encoding.UTF8.GetBytes("This is a dummy file");
            var expectedTeamLogo = new FormFile(new MemoryStream(byteArray), 0, byteArray.Length, "Data", "image.jpg");
            var expectedTeamLogoExtension = Path.GetExtension(expectedTeamLogo.FileName);

            var expectedTeamId = Guid.NewGuid();
            var expectedTeamName = "Name";
            var expectedSubcategoryId = Guid.NewGuid();
            var expectedLocation = "Location";
            var expectedTeamIsHidden = true;
            var expectedTeamOrderIndex = 1;
            var expectedByteArray = byteArray;
            var fileExtension = expectedTeamLogoExtension;

            EditTeamModel teamModel = new()
            {
                Id = expectedTeamId,
                Name = expectedTeamName,
                Location = expectedLocation,
                SubcategoryId = expectedSubcategoryId,
                IsHidden = expectedTeamIsHidden,
                OrderIndex = expectedTeamOrderIndex,
                TeamLogo = expectedTeamLogo
            };

            // Act
            await _service.EditTeamAsync(teamModel);

            // Assert
            _teamRepository.Verify(repository => repository.EditTeamAsync(It.Is<Team>(team =>
                (team.Id == expectedTeamId)
                && (team.Name == expectedTeamName) 
                && (team.Location == expectedLocation)
                && (team.SubcategoryId == expectedSubcategoryId)
                && (team.IsHidden == expectedTeamIsHidden)
                && (team.OrderIndex == expectedTeamOrderIndex))));

            _teamLogoRepository.Verify(repository => repository.EditTeamLogoAsync(It.Is<TeamLogo>(teamLogo =>
                (byteArray == expectedByteArray)
                && (fileExtension == expectedTeamLogoExtension)
                && (teamModel.Id == expectedTeamId)
                )));
        }
    }
}
