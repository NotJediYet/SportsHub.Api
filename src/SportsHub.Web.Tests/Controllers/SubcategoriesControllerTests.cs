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
    public class SubcategoriesControllerTests
    {
        private readonly Mock<ISubcategoryService> _service;
        private readonly Mock<IValidator<CreateSubcategoryModel>> _createSubcategoryValidator;
        private readonly Mock<IValidator<EditSubcategoryModel>> _editSubcategoryValidator;
        private readonly SubcategoriesController _controller;

        public SubcategoriesControllerTests()
        {
            _service = new Mock<ISubcategoryService>();
            _createSubcategoryValidator = new Mock<IValidator<CreateSubcategoryModel>>();
            _editSubcategoryValidator = new Mock<IValidator<EditSubcategoryModel>>();

            _controller = new SubcategoriesController(_service.Object, _createSubcategoryValidator.Object, _editSubcategoryValidator.Object);
        }

        [Fact]
        public async Task CreateSubcategory_WhenModelIsValid_ReturnsOkResult()
        {
            // Arrange
            var model = new CreateSubcategoryModel
            {
                Name = "Name",
                CategoryId = Guid.NewGuid()
            };
            var validationResult = new ValidationResult();

            _createSubcategoryValidator.Setup(validator => validator.ValidateAsync(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateSubcategory(model);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateSubcategory_WhenModelIsInvalid_ReturnsBadRequestResult()
        {
            // Arrange
            var model = new CreateSubcategoryModel
            {
                Name = string.Empty,
                CategoryId = Guid.NewGuid()
            };

            var validationFailure = new ValidationFailure(nameof(model.Name), Errors.SubcategoryNameCannotBeEmpty);
            var validationResult = new ValidationResult(new[] { validationFailure });

            _createSubcategoryValidator.Setup(validator => validator.ValidateAsync(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateSubcategory(model);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(validationFailure.ErrorMessage, objectResult.Value);
        }

        [Fact]
        public async Task GetSubcategories_ReturnsOkObjectResultWithSubcategories()
        {
            // Arrange
            var expectedSubcategories = GetSubcategories();

            _service.Setup(service => service.GetSubcategoriesAsync())
                .ReturnsAsync(expectedSubcategories);

            // Act
            var result = await _controller.GetSubcategories();

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            var actualSubcategories = Assert.IsAssignableFrom<IEnumerable<Subcategory>>(objectResult.Value);
            Assert.Equal(expectedSubcategories, actualSubcategories);
        }

        [Fact]
        public async Task GetSubcategory_WhenSubcategoryDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var subcategoryId = Guid.NewGuid();

            _service.Setup(service => service.GetSubcategoryByIdAsync(subcategoryId))
                .ReturnsAsync((Subcategory)null);

            // Act
            var result = await _controller.GetSubcategory(subcategoryId);

            // Assert
            var objectResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetSubcategory_WhenSubcategoryExists_ReturnsOkObjectResultWithSubcategory()
        {
            // Arrange
            var expectedSubcategoryId = Guid.NewGuid();

            var expectedSubcategory = new Subcategory { Name = "Name", CategoryId = Guid.NewGuid() };
            expectedSubcategory.Id = expectedSubcategoryId;

            _service.Setup(service => service.GetSubcategoryByIdAsync(expectedSubcategoryId))
               .ReturnsAsync(expectedSubcategory);

            // Act
            var result = await _controller.GetSubcategory(expectedSubcategoryId);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            var actualCategory = Assert.IsType<Subcategory>(objectResult.Value);
            Assert.Equal(expectedSubcategory.Name, actualCategory.Name);
            Assert.Equal(expectedSubcategory.Id, actualCategory.Id);
            Assert.Equal(expectedSubcategory.CategoryId, actualCategory.CategoryId);
        }

        private IEnumerable<Subcategory> GetSubcategories()
        {
            IEnumerable<Subcategory> subcategories = new List<Subcategory>
            {
                new Subcategory { Name = "Name1", CategoryId = Guid.NewGuid() },
                new Subcategory { Name = "Name2", CategoryId = Guid.NewGuid() },
                new Subcategory { Name = "Name3", CategoryId = Guid.NewGuid() },
            };
            return subcategories;
        }

        [Fact]
        public async Task EditSubcategory_WhenModelIsValid_ReturnsOkResult()
        {
            // Arrange
            var model = new EditSubcategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                CategoryId = Guid.NewGuid(),
                IsHidden = false,
                OrderIndex = 0,
            };
            var validationResult = new ValidationResult();

            _editSubcategoryValidator.Setup(validator => validator.ValidateAsync(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.EditSubcategory(model);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task EditSubcategory_WhenModelIsInvalid_ReturnsBadRequestResult()
        {
            var model = new EditSubcategoryModel
            {
                Id = Guid.NewGuid(),
                Name = "",
                CategoryId = Guid.NewGuid(),
                IsHidden = false,
                OrderIndex = 0,
            };

            var validationFailure = new ValidationFailure(nameof(model.Name), Errors.SubcategoryNameCannotBeEmpty);
            var validationResult = new ValidationResult(new[] { validationFailure });

            _editSubcategoryValidator.Setup(validator => validator.ValidateAsync(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.EditSubcategory(model);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(validationFailure.ErrorMessage, objectResult.Value);
        }
    }
}
