using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;
using SportsHub.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SportsHub.Web.Tests.Controllers
{
    public class TeamsControllerTests
    {
        private readonly Mock<ITeamService> _service;
        private readonly Mock<IValidator<CreateTeamModel>> _validator;
        private readonly TeamsController _controller;

        public TeamsControllerTests()
        {
            _service = new Mock<ITeamService>();
            _validator = new Mock<IValidator<CreateTeamModel>>();

            _controller = new TeamsController(_service.Object, _validator.Object);
        }

        [Fact]
        public async Task CreateTeam_HasValidValues_ReturnsOkResult()
        {
            // Arrange
            var team = new CreateTeamModel
            {
                Name = "Name",
                SubcategoryId = Guid.NewGuid()
            };

            _validator.Setup(validator => validator.ValidateAsync(It.IsAny<CreateTeamModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Act
            var result = await _controller.CreateTeam(team);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateTeam_HasInvalidValues_ReturnsBadRequestResult()
        {
            // Arrange
            var team = new CreateTeamModel
            {
                Name = string.Empty,
                SubcategoryId = Guid.NewGuid()
            };

            var validationFailure = new ValidationFailure(nameof(CreateTeamModel.Name), "Something went wrong");
            _validator.Setup(validator => validator.ValidateAsync(It.IsAny<CreateTeamModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[] { validationFailure }));

            // Act
            var result = await _controller.CreateTeam(team);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetTeams_ReturnsOkObjectResultWithCollectionOfTeams()
        {
            // Arrange
            _service.Setup(service => service.GetTeamsAsync())
                .ReturnsAsync(GetTestTeams());

            // Act
            var result = await _controller.GetTeams();

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
            var teams = Assert.IsAssignableFrom<IEnumerable<Team>>(objectResult.Value);
            Assert.Equal(GetTestTeams().Count(), teams.Count());
        }

        [Fact]
        public async Task GetTeam_HasInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var testTeamId = Guid.Empty;

            // Act
            var result = await _controller.GetTeam(testTeamId);

            // Assert
            var objectResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetTeam_HasValidId_ReturnsOkObjectResultWithTeam()
        {
            // Arrange
            var expectedTeam = GetTestTeams().FirstOrDefault();

            _service.Setup(service => service.GetTeamByIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(expectedTeam);

            // Act
            var result = await _controller.GetTeam(Guid.NewGuid());

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
            var subcategory = Assert.IsType<Team>(objectResult.Value);
            Assert.Equal(expectedTeam.Name, subcategory.Name);
            Assert.Equal(expectedTeam.SubcategoryId, subcategory.SubcategoryId);
            Assert.Equal(expectedTeam.Id, subcategory.Id);
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
