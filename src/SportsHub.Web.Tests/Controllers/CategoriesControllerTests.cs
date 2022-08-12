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
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _service;
        private readonly Mock<IValidator<CreateCategoryModel>> _validator;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            _service = new Mock<ICategoryService>();
            _validator = new Mock<IValidator<CreateCategoryModel>>();
            
            _controller = new CategoriesController(_service.Object, _validator.Object);
        }

        [Fact]
        public async Task CreateCategory_WhenModelIsValid_ReturnsOkResult()
        {
            // Arrange
            var model = new CreateCategoryModel
            {
                Name = "Name",
            };
            var validationResult = new ValidationResult();

            _validator.Setup(validator => validator.ValidateAsync(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateCategory(model);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateCategory_WhenModelIsInvalid_ReturnsBadRequestResult()
        {
            // Arrange
            var model = new CreateCategoryModel
            {
                Name = string.Empty,
            };

            var validationFailure = new ValidationFailure(nameof(model.Name), Errors.CategoryNameCannotBeEmpty);
            var validationResult = new ValidationResult(new[] { validationFailure });

            _validator.Setup(validator => validator.ValidateAsync(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateCategory(model);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(validationFailure.ErrorMessage, objectResult.Value);
        }

        [Fact]
        public async Task GetCategories_ReturnsOkObjectResultWithCategories()
        {
            // Arrange
            var expectedCategories = GetCategories();

            _service.Setup(service => service.GetCategoriesAsync())
                .ReturnsAsync(expectedCategories);

            // Act
            var result = await _controller.GetCategories();

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var actualCategories = Assert.IsAssignableFrom<IEnumerable<Category>>(objectResult.Value);
            Assert.Equal(expectedCategories, actualCategories);
        }

        [Fact]
        public async Task GetCategory_WhenCategoryDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            _service.Setup(service => service.GetCategoryByIdAsync(categoryId))
                .ReturnsAsync((Category) null);

            // Act
            var result = await _controller.GetCategory(categoryId);

            // Assert
            var objectResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetCategory_WhenCategoryExists_ReturnsOkObjectResultWithCategory()
        {
            // Arrange
            var expectedCategoryId = Guid.NewGuid();

            var expectedCategory = new Category(name: "Name");
            expectedCategory.Id = expectedCategoryId;

            _service.Setup(service => service.GetCategoryByIdAsync(expectedCategoryId))
               .ReturnsAsync(expectedCategory);

            // Act
            var result = await _controller.GetCategory(expectedCategoryId);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            var actualCategory = Assert.IsType<Category>(objectResult.Value);
            Assert.Equal(expectedCategory.Name, actualCategory.Name);
            Assert.Equal(expectedCategory.Id, actualCategory.Id);
        }

        private IEnumerable<Category> GetCategories()
        {
            IEnumerable<Category> categories = new List<Category>
            {
                new Category("Name1"),
                new Category("Name2"),
                new Category("Name3")
            };
            return categories;
        }
    }
}
