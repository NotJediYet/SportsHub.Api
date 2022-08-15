using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;
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
        public async void CreateSubcategoryModel_WhenNameIsEmpty_ReturnsValidationResultWithError()
        {
            // Arrange
            var subcategory = new CreateSubcategoryModel
            {
                Name = string.Empty,
                CategoryId = Guid.NewGuid()
            };
            var expectedErrorMessage = Errors.SubcategoryNameCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(subcategory);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateSubcategoryModel_WhenNameIsNotUnique_ReturnsValidationResultWithError()
        {
            // Arrange
            var subcategory = new CreateSubcategoryModel
            {
                Name = "Name",
                CategoryId = Guid.NewGuid()
            };
            var expectedErrorMessage = Errors.SubcategoryNameIsNotUnique;

            _subcategoryService.Setup(service => service.DoesSubcategoryAlreadyExistByNameAsync(subcategory.Name))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.ValidateAsync(subcategory);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateSubcategoryModel_WhenCategoryIdIsEmpty_ReturnsValidationResultWithError()
        {
            // Arrange
            var subcategory = new CreateSubcategoryModel
            {
                Name = "Name",
                CategoryId = Guid.Empty
            };
            var expectedErrorMessage = Errors.CategoryIdCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(subcategory);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateSubcategoryModel_WhenCategoryDoesNotExist_ReturnsValidationResultWithError()
        {
            // Arrange
            var subcategory = new CreateSubcategoryModel
            {
                Name = "Name",
                CategoryId = Guid.NewGuid()
            };
            var expectedErrorMessage = Errors.CategoryDoesNotExist;

            _categoryService.Setup(service => service.DoesCategoryAlreadyExistByIdAsync(subcategory.CategoryId))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.ValidateAsync(subcategory);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateSubcategoryModel_WhenModelIsValid_ReturnsSuccessValidationResult()
        {
            // Arrange
            var subcategory = new CreateSubcategoryModel
            {
                Name = "Name",
                CategoryId = Guid.NewGuid()
            };

            _subcategoryService.Setup(service => service.DoesSubcategoryAlreadyExistByNameAsync(subcategory.Name))
                .ReturnsAsync(false);
            _categoryService.Setup(service => service.DoesCategoryAlreadyExistByIdAsync(subcategory.CategoryId))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.ValidateAsync(subcategory);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}
