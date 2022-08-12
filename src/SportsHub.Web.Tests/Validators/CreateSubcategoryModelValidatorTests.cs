using FluentValidation.Results;
using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Web.Validators;
using System;
using Xunit;

namespace SportsHub.Web.Tests.Validators
{
    public class CreateSubcategoryModelValidatorTests
    {
        private readonly Mock<ICategoryService> _categoryService;
        private readonly Mock<ISubcategoryService> _subcategoryService;
        private readonly CreateSubcategoryModelValidator _validator;

        public CreateSubcategoryModelValidatorTests()
        {
            _categoryService = new Mock<ICategoryService>();
            _subcategoryService = new Mock<ISubcategoryService>();

            _validator = new CreateSubcategoryModelValidator(_categoryService.Object, _subcategoryService.Object);
        }

        [Fact]
        public async void CreateSubcategoryModel_HasEmptyName_ReturnsValidationResultWithError()
        {
            // Arrange
            var subcategory = new CreateSubcategoryModel
            {
                Name = string.Empty,
                CategoryId = Guid.NewGuid()
            };

            // Act
            var result = await _validator.ValidateAsync(subcategory);

            // Assert
            Assert.IsType<ValidationResult>(result);
            Assert.False(result.IsValid);
        }

        [Fact]
        public async void CreateSubcategoryModel_HasExistingName_ReturnsValidationResultWithError()
        {
            // Arrange
            var subcategory = new CreateSubcategoryModel
            {
                Name = "Test name",
                CategoryId = Guid.NewGuid()
            };

            _subcategoryService.Setup(service => service.DoesSubcategoryAlreadyExistByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.ValidateAsync(subcategory);

            // Assert
            Assert.IsType<ValidationResult>(result);
            Assert.False(result.IsValid);
        }

        [Fact]
        public async void CreateSubcategoryModel_HasEmptyCategoryId_ReturnsValidationResultWithError()
        {
            // Arrange
            var subcategory = new CreateSubcategoryModel
            {
                Name = "Test name",
                CategoryId = Guid.Empty
            };

            // Act
            var result = await _validator.ValidateAsync(subcategory);

            // Assert
            Assert.IsType<ValidationResult>(result);
            Assert.False(result.IsValid);
        }

        [Fact]
        public async void CreateSubcategoryModel_HasNotExistingCategoryId_ReturnsValidationResultWithError()
        {
            // Arrange
            var subcategory = new CreateSubcategoryModel
            {
                Name = "Test name",
                CategoryId = Guid.NewGuid()
            };

            _categoryService.Setup(service => service.DoesCategoryAlredyExistByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.ValidateAsync(subcategory);

            // Assert
            Assert.IsType<ValidationResult>(result);
            Assert.False(result.IsValid);
        }
    }
}
