using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;
using SportsHub.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task CreateCategory_HasValidValues_ReturnsOkResult()
        {
            // Arrange
            var category = new CreateCategoryModel
            {
                Name = "Name",
            };

            _validator.Setup(validator => validator.ValidateAsync(It.IsAny<CreateCategoryModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Act
            var result = await _controller.CreateCategory(category);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateCategory_HasInvalidValues_ReturnsBadRequestResult()
        {
            // Arrange
            var category = new CreateCategoryModel
            {
                Name = string.Empty,
            };

            var validationFailure = new ValidationFailure(nameof(CreateCategoryModel.Name), "Something went wrong");
            _validator.Setup(validator => validator.ValidateAsync(It.IsAny<CreateCategoryModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[] { validationFailure }));

            // Act
            var result = await _controller.CreateCategory(category);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetCategories_ReturnsOkObjectResultWithCollectionOfCategories()
        {
            // Arrange
            _service.Setup(service => service.GetCategoriesAsync())
                .ReturnsAsync(GetTestCategories());
                
            // Act
            var result = await _controller.GetCategories();

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
            var categories = Assert.IsAssignableFrom<IEnumerable<Category>>(objectResult.Value);
            Assert.Equal(GetTestCategories().Count(), categories.Count());
        }
                
        [Fact]
        public async Task GetCategory_HasInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var testCategoryId = Guid.Empty;

            // Act
            var result = await _controller.GetCategory(testCategoryId);

            // Assert
            var objectResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetCategory_HasValidId_ReturnsOkObjectResultWithCategory()
        {
            // Arrange
            var testCategory = GetTestCategories().FirstOrDefault();

            _service.Setup(service => service.GetCategoryByIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(testCategory);

            // Act
            var result = await _controller.GetCategory(Guid.NewGuid());

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
            var category = Assert.IsType<Category>(objectResult.Value);
            Assert.Equal(testCategory.Name, category.Name);
            Assert.Equal(testCategory.Id, category.Id);
        }

        private IEnumerable<Category> GetTestCategories()
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
