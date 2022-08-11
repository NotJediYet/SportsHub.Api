using FluentValidation.Results;
using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Web.Validators;
using System;
using Xunit;

namespace SportsHub.Web.Tests.Validators
{
    public class CreateTeamModelValidatorTest
    {
        private readonly Mock<ISubcategoryService> _subcategoryService;
        private readonly Mock<ITeamService> _teamService;
        private readonly CreateTeamModelValidator _validator;

        public CreateTeamModelValidatorTest()
        {
            _subcategoryService = new Mock<ISubcategoryService>();
            _teamService = new Mock<ITeamService>();

            _validator = new CreateTeamModelValidator(_subcategoryService.Object, _teamService.Object);
        }

        [Fact]
        public async void CreateTeamModel_HasEmptyName_ReturnsValidationResultWithError()
        {
            // Arrange
            var team = new CreateTeamModel
            {
                Name = string.Empty,
                SubcategoryId = Guid.NewGuid()
            };

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.IsType<ValidationResult>(result);
            Assert.False(result.IsValid);
        }

        [Fact]
        public async void CreateTeamModel_HasExistingName_ReturnsValidationResultWithError()
        {
            // Arrange
            var team = new CreateTeamModel
            {
                Name = "Test name",
                SubcategoryId = Guid.NewGuid()
            };

            _teamService.Setup(service => service.DoesTeamAlreadyExistByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.IsType<ValidationResult>(result);
            Assert.False(result.IsValid);
        }

        [Fact]
        public async void CreateTeamModel_HasEmptyCategoryId_ReturnsValidationResultWithError()
        {
            // Arrange
            var team = new CreateTeamModel
            {
                Name = "Test name",
                SubcategoryId = Guid.Empty
            };

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.IsType<ValidationResult>(result);
            Assert.False(result.IsValid);
        }

        [Fact]
        public async void CreateTeamModel_HasNotExistingCategoryId_ReturnsValidationResultWithError()
        {
            // Arrange
            var team = new CreateTeamModel
            {
                Name = "Test name",
                SubcategoryId = Guid.NewGuid()
            };

            _subcategoryService.Setup(service => service.DoesSubcategoryAlredyExistByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.IsType<ValidationResult>(result);
            Assert.False(result.IsValid);
        }
    }
}
