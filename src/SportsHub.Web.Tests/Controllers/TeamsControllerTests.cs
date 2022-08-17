using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;
using SportsHub.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SportsHub.Web.Tests.Controllers
{
    public class TeamsControllerTests
    {
        private readonly Mock<ITeamService> _service;
        private readonly Mock<IValidator<CreateTeamModel>> _validator;
        private readonly Mock<IValidator<EditTeamModel>> _validatorEditTeamModel;
        private readonly TeamsController _controller;

        public TeamsControllerTests()
        {
            _service = new Mock<ITeamService>();
            _validator = new Mock<IValidator<CreateTeamModel>>();
            _validatorEditTeamModel = new Mock<IValidator<EditTeamModel>>();

            _controller = new TeamsController(_service.Object, _validator.Object, _validatorEditTeamModel.Object);
        }

        [Fact]
        public async Task CreateTeam_WhenModelIsValid_ReturnsOkResult()
        {
            // Arrange
            var model = new CreateTeamModel
            {
                Name = "Name",
                SubcategoryId = Guid.NewGuid()
            };
            var validationResult = new ValidationResult();

            _validator.Setup(validator => validator.ValidateAsync(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateTeam(model);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateTeam_WhenModelIsInvalid_ReturnsBadRequestResult()
        {
            // Arrange
            var model = new CreateTeamModel
            {
                Name = string.Empty,
                SubcategoryId = Guid.NewGuid()
            };

            var validationFailure = new ValidationFailure(nameof(model.Name), Errors.TeamNameCannotBeEmpty);
            var validationResult = new ValidationResult(new[] { validationFailure });

            _validator.Setup(validator => validator.ValidateAsync(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateTeam(model);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(validationFailure.ErrorMessage, objectResult.Value);
        }

        [Fact]
        public async Task GetTeams_ReturnsOkObjectResultWithTeams()
        {
            // Arrange
            var expectedTeams = GetTeams();

            _service.Setup(service => service.GetTeamsAsync())
                .ReturnsAsync(expectedTeams);

            // Act
            var result = await _controller.GetTeams();

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            var actualTeams = Assert.IsAssignableFrom<IEnumerable<Team>>(objectResult.Value);
            Assert.Equal(expectedTeams, actualTeams);
        }

        [Fact]
        public async Task GetTeam_WhenTeamDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var teamId = Guid.NewGuid();

            _service.Setup(service => service.GetTeamByIdAsync(teamId))
                .ReturnsAsync((Team)null);

            // Act
            var result = await _controller.GetTeam(teamId);

            // Assert
            var objectResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetTeam_WhenTeamExists_ReturnsOkObjectResultWithTeam()
        {
            // Arrange
            var expectedTeamId = Guid.NewGuid();

            var expectedTeam = new Team(name: "Name", subcategoryId: Guid.NewGuid(), location: "Location");
            expectedTeam.Id = expectedTeamId;

            _service.Setup(service => service.GetTeamByIdAsync(expectedTeamId))
               .ReturnsAsync(expectedTeam);

            // Act
            var result = await _controller.GetTeam(expectedTeamId);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            var actualCategory = Assert.IsType<Team>(objectResult.Value);
            Assert.Equal(expectedTeam.Name, actualCategory.Name);
            Assert.Equal(expectedTeam.Id, actualCategory.Id);
            Assert.Equal(expectedTeam.SubcategoryId, expectedTeam.SubcategoryId);
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
