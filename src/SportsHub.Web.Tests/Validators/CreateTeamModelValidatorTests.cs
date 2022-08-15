using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;
using SportsHub.Web.Validators;
using System;
using Xunit;

namespace SportsHub.Web.Tests.Validators
{
    public class CreateTeamModelValidatorTests
    {
        private readonly Mock<ISubcategoryService> _subcategoryService;
        private readonly Mock<ITeamService> _teamService;
        private readonly CreateTeamModelValidator _validator;

        public CreateTeamModelValidatorTests()
        {
            _subcategoryService = new Mock<ISubcategoryService>();
            _teamService = new Mock<ITeamService>();
            _validator = new CreateTeamModelValidator(_subcategoryService.Object, _teamService.Object);
        }

        [Fact]
        public async void CreateTeamModel_WhenNameIsEmpty_ReturnsValidationResultWithError()
        {
            // Arrange
            var team = new CreateTeamModel
            {
                Name = string.Empty,
                SubcategoryId = Guid.NewGuid()
            };
            var expectedErrorMessage = Errors.TeamNameCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateTeamModel_WhenNameIsNotUnique_ReturnsValidationResultWithError()
        {
            // Arrange
            var team = new CreateTeamModel
            {
                Name = "Name",
                SubcategoryId = Guid.NewGuid()
            };
            var expectedErrorMessage = Errors.TeamNameIsNotUnique;

            _teamService.Setup(service => service.DoesTeamAlreadyExistByNameAsync(team.Name))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateTeamModel_WhenSubcategoryIdIsEmpty_ReturnsValidationResultWithError()
        {
            // Arrange
            var team = new CreateTeamModel
            {
                Name = "Name",
                SubcategoryId = Guid.Empty
            };
            var expectedErrorMessage = Errors.SubcategoryIdCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateTeamModel_WhenSubcategoryDoesNotExist_ReturnsValidationResultWithError()
        {
            // Arrange
            var team = new CreateTeamModel
            {
                Name = "Name",
                SubcategoryId = Guid.NewGuid()
            };
            var expectedErrorMessage = Errors.SubcategoryDoesNotExist;

            _subcategoryService.Setup(service => service.DoesSubcategoryAlreadyExistByIdAsync(team.SubcategoryId))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateTeamModel_WhenModelIsValid_ReturnsSuccessValidationResult()
        {
            // Arrange
            var team = new CreateTeamModel
            {
                Name = "Name",
                SubcategoryId = Guid.NewGuid()
            };

            _teamService.Setup(service => service.DoesTeamAlreadyExistByNameAsync(team.Name))
                .ReturnsAsync(false);
            _subcategoryService.Setup(service => service.DoesSubcategoryAlreadyExistByIdAsync(team.SubcategoryId))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}
