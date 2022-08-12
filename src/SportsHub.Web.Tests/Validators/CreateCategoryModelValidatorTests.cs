using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;
using SportsHub.Web.Validators;
using Xunit;

namespace SportsHub.Web.Tests.Validators
{
    public class CreateCategoryModelValidatorTests
    {
        private readonly Mock<ICategoryService> _service;
        private readonly CreateCategoryModelValidator _validator;

        public CreateCategoryModelValidatorTests()
        {
            _service = new Mock<ICategoryService>();
            _validator = new CreateCategoryModelValidator(_service.Object);
        }

        [Fact]
        public async void CreateCategoryModel_WhenNameIsEmpty_ReturnsValidationResultWithError()
        {
            // Arrange
            var category = new CreateCategoryModel
            {
                Name = string.Empty
            };
            var expectedErrorMessage = Errors.CategoryNameCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(category);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateCategoryModel_WhenNameIsNotUnique_ReturnsValidationResultWithError()
        {
            // Arrange
            var category = new CreateCategoryModel
            {
                Name = "Name"
            };
            var expectedErrorMessage = Errors.CategoryNameIsNotUnique;

            _service.Setup(service => service.DoesCategoryAlreadyExistByNameAsync(category.Name))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.ValidateAsync(category);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateCategoryModel_WhenModelIsValid_ReturnsSuccessValidationResult()
        {
            // Arrange
            var category = new CreateCategoryModel
            {
                Name = "Name"
            };

            _service.Setup(service => service.DoesCategoryAlreadyExistByNameAsync(category.Name))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.ValidateAsync(category);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}
