using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;
using SportsHub.Web.Validators;
using System;
using Xunit;

namespace SportsHub.Web.Tests.Validators
{
    public class EditSubcategoryModelValidatorTests
    {
        private readonly Mock<ICategoryService> _categoryService;
        private readonly Mock<ISubcategoryService> _subcategoryService;
        private readonly EditSubcategoryModelValidator _validator;

        public EditSubcategoryModelValidatorTests()
        {
            _categoryService = new Mock<ICategoryService>();
            _subcategoryService = new Mock<ISubcategoryService>();
            _validator = new EditSubcategoryModelValidator(_categoryService.Object, _subcategoryService.Object);
        }

        [Fact]
        public async void EditSubcategoryModel_WhenSubcategoryIdIsEmpty_ReturnsValidationResultWithError()
        {
            // Arrange
            var subcategory = new EditSubcategoryModel
            {
                Id = Guid.Empty,
                Name = "Name",
                CategoryId = Guid.NewGuid(),
                IsHidden = true,
                OrderIndex = 0,
            };
            var expectedErrorMessage = Errors.SubcategoryIdCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(subcategory);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditSubcategoryModel_WhenSubcategoryIdDoesNotExist_ReturnsValidationResultWithError()
        {
            // Arrange
            var subcategory = new EditSubcategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                CategoryId = Guid.NewGuid(),
                IsHidden = true,
                OrderIndex = 0,
            };

            var expectedErrorMessage = Errors.SubcategoryDoesNotExist;

            _subcategoryService.Setup(service => service.DoesSubcategoryAlreadyExistByIdAsync(subcategory.Id))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.ValidateAsync(subcategory);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditSubcategoryModel_WhenSubcategoryNameIsEmpty_ReturnsValidationResultWithError()
        {
            // Arrange
            var subcategory = new EditSubcategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "",
                CategoryId = Guid.NewGuid(),
                IsHidden = true,
                OrderIndex = 0,
            };

            var expectedErrorMessage = Errors.SubcategoryNameCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(subcategory);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditSubcategoryModel_WhenSubcategoryNameAlreadyExist_ReturnsValidationResultWithError()
        {
            // Arrange
            var subcategory = new EditSubcategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                CategoryId = Guid.NewGuid(),
                IsHidden = true,
                OrderIndex = 0,
            };

            var expectedErrorMessage = Errors.SubcategoryNameIsNotUnique;

            _subcategoryService.Setup(service => service.GetSubcategoryIdByNameAsync(subcategory.Name))
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _validator.ValidateAsync(subcategory);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditSubcategoryModel_WhenCategoryIdIsEmpty_ReturnsValidationResultWithError()
        {
            var subcategory = new EditSubcategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                CategoryId = Guid.Empty,
                IsHidden = true,
                OrderIndex = 0,
            };
            var expectedErrorMessage = Errors.CategoryIdCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(subcategory);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditSubcategoryModel_WhenCategoryDoesNotExist_ReturnsValidationResultWithError()
        {
            // Arrange
            var subcategory = new EditSubcategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                CategoryId = Guid.NewGuid(),
                IsHidden = true,
                OrderIndex = 0,
            };

            var expectedErrorMessage = Errors.SubcategoryDoesNotExist;

            _categoryService.Setup(service => service.DoesCategoryAlreadyExistByIdAsync(subcategory.CategoryId))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.ValidateAsync(subcategory);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditSubcategoryModel_WhenModelIsValid_ReturnsSuccessValidationResult()
        {
            // Arrange
            var subcategory = new EditSubcategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                CategoryId = Guid.NewGuid(),
                IsHidden = true,
                OrderIndex = 0,
            };

            _subcategoryService.Setup(service => service.DoesSubcategoryAlreadyExistByIdAsync(subcategory.Id))
            .ReturnsAsync(true);
            _subcategoryService.Setup(service => service.GetSubcategoryIdByNameAsync(subcategory.Name))
            .ReturnsAsync(Guid.Empty);
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
