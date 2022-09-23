using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;
using SportsHub.Web.Validators;
using System;
using Xunit;

namespace SportsHub.Web.Tests.Validators
{
    public class EditCategoryModelValidatorTests
    {
        private readonly Mock<ICategoryService> _service;
        private readonly EditCategoryModelValidator _validator;

        public EditCategoryModelValidatorTests()
        {
            _service = new Mock<ICategoryService>();
            _validator = new EditCategoryModelValidator(_service.Object);
        }

        [Fact]
        public async void EditCategoryModel_WhenCategoryIdIsEmpty_ReturnsValidationResultWithError()
        {
            var category = new EditCategoryModel
            {
                Id = Guid.Empty,
                Name = "Name",
                IsStatic = true,
                IsHidden = false,
                OrderIndex = 0
            };

            var expectedErrorMessage = Errors.CategoryIdCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(category);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditCategoryModel_WhenCategoryIdDoesNotExist_ReturnsValidationResultWithError()
        {
            var category = new EditCategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                IsStatic = true,
                IsHidden = false,
                OrderIndex = 0
            };

            var expectedErrorMessage = Errors.CategoryDoesNotExist;

            _service.Setup(service => service.DoesCategoryAlreadyExistByIdAsync(category.Id))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.ValidateAsync(category);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditCategoryModel_WhenCategoryNameIsEmpty_ReturnsValidationResultWithError()
        {
            var category = new EditCategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "",
                IsStatic = true,
                IsHidden = false,
                OrderIndex = 0
            };

            var expectedErrorMessage = Errors.CategoryNameCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(category);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditCategoryModel_WhenCategoryNameAlreadyExist_ReturnsValidationResultWithError()
        {
            var category = new EditCategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                IsStatic = true,
                IsHidden = false,
                OrderIndex = 0
            };

            var expectedErrorMessage = Errors.CategoryNameIsNotUnique;

            _service.Setup(service => service.GetCategoryIdByNameAsync(category.Name))
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _validator.ValidateAsync(category);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditCategoryModel_WhenModelIsValid_ReturnsSuccessValidationResult()
        {
            var category = new EditCategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                IsStatic = true,
                IsHidden = false,
                OrderIndex = 0
            };

            _service.Setup(service => service.DoesCategoryAlreadyExistByIdAsync(category.Id))
            .ReturnsAsync(true);
            _service.Setup(service => service.GetCategoryIdByNameAsync(category.Name))
            .ReturnsAsync(Guid.Empty);

            // Act
            var result = await _validator.ValidateAsync(category);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}
