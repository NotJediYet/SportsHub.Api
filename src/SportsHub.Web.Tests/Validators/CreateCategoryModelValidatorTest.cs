using FluentValidation.Results;
using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Web.Validators;
using Xunit;

namespace SportsHub.Web.Tests.Validators
{
    public class CreateCategoryModelValidatorTest
    {
        private readonly Mock<ICategoryService> _service;
        private readonly CreateCategoryModelValidator _validator;

        public CreateCategoryModelValidatorTest()
        {
            _service = new Mock<ICategoryService>();
            _validator = new CreateCategoryModelValidator(_service.Object);
        }

        [Fact]
        public async void CreateCategoryModel_HasEmptyName_ReturnsValidationResultWithError()
        {
            // Arrange
            var category = new CreateCategoryModel
            {
                Name = string.Empty
            };

            // Act
            var result = await _validator.ValidateAsync(category);

            // Assert
            Assert.IsType<ValidationResult>(result); 
            Assert.False(result.IsValid);
        }

        [Fact]
        public async void CreateCategoryModel_HasExistingName_ReturnsValidationResultWithError()
        {
            // Arrange
            var category = new CreateCategoryModel
            {
                Name = "Test name"
            };

            _service.Setup(service => service.DoesCategoryAlreadyExistByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.ValidateAsync(category);

            // Assert
            Assert.IsType<ValidationResult>(result);
            Assert.False(result.IsValid);
        }
    }
}
