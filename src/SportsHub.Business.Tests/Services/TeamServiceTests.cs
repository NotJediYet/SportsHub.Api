using Moq;
using SportsHub.Business.Repositories;
using SportsHub.Business.Services;
using SportsHub.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace SportsHub.Business.Tests.Services
{
    public class TeamServiceTests
    {
        private readonly Mock<ITeamRepository> _repository1;
        private readonly Mock<ITeamLogoRepository> _repository2;
        private readonly ITeamService _service;

        public TeamServiceTests()
        {
            _repository1 = new Mock<ITeamRepository>();
            _repository2 = new Mock<ITeamLogoRepository>();
            _service = new TeamService(_repository1.Object, _repository2.Object);
        }

        [Fact]
        public async Task GetTeamsAsync_ReturnsExpectedTeams()
        {
            // Arrange
            var expectedTeams = GetTeams();

            _repository1.Setup(repository => repository.GetTeamsAsync())
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

            _repository1.Setup(repo => repo.GetTeamByIdAsync(expectedTeamId))
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
            var expectedlocation = "Location";
            var imageStream = new MemoryStream(GenerateImageByteArray());
            IFormFile expectedTeamLogo = new FormFile(imageStream, 0, imageStream.Length, "UnitTest", "UnitTest.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            // Act
            await _service.CreateTeamAsync(expectedTeamName, expectedSubcategoryId, expectedlocation, expectedTeamLogo);

            // Assert
            _repository1.Verify(repository => repository.AddTeamAsync(It.Is<Team>(team =>
            (team.Name == expectedTeamName) && (team.SubcategoryId == expectedSubcategoryId))));
        }

        [Fact]
        public async Task DoesTeamAlreadyExistByNameAsync_WhenTeamExists_ReturnsTrue()
        {
            // Arrange
            var teamName = "Name";

            _repository1.Setup(repository => repository.DoesTeamAlreadyExistByNameAsync(teamName))
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

            _repository1.Setup(repository => repository.DoesTeamAlreadyExistByNameAsync(teamName))
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
        private byte[] GenerateImageByteArray(int width = 50, int height = 50)
        {
            Bitmap bitmapImage = new Bitmap(width, height);
            Graphics imageData = Graphics.FromImage(bitmapImage);
            imageData.DrawLine(new Pen(Color.Blue), 0, 0, width, height);

            MemoryStream memoryStream = new MemoryStream();
            byte[] byteArray;

            using (memoryStream)
            {
                bitmapImage.Save(memoryStream, ImageFormat.Jpeg);
                byteArray = memoryStream.ToArray();
            }
            return byteArray;
        }
    }
}
