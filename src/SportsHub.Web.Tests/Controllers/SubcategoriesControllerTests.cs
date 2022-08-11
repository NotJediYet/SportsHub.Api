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
    public class SubcategoriesControllerTests
    {
        private readonly Mock<ISubcategoryService> _service;
        private readonly Mock<IValidator<CreateSubcategoryModel>> _validator;
        private readonly SubcategoriesController _controller;

        public SubcategoriesControllerTests()
        {
            _service = new Mock<ISubcategoryService>();
            _validator = new Mock<IValidator<CreateSubcategoryModel>>();

            _controller = new SubcategoriesController(_service.Object, _validator.Object);
        }

        [Fact]
        public async Task CreateSubcategory_HasValidValues_ReturnsOkResult()
        {
            // Arrange
            var subcategory = new CreateSubcategoryModel
            {
                Name = "Name",
                CategoryId = Guid.NewGuid()
            };

            _validator.Setup(validator => validator.ValidateAsync(It.IsAny<CreateSubcategoryModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult());

            // Act
            var result = await _controller.CreateSubcategory(subcategory);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateSubcategory_HasInvalidValues_ReturnsBadRequestResult()
        {
            // Arrange
            var subcategory = new CreateSubcategoryModel
            {
                Name = string.Empty,
                CategoryId = Guid.NewGuid()
            };

            var validationFailure = new ValidationFailure(nameof(CreateSubcategoryModel.Name), "Something went wrong");
            _validator.Setup(validator => validator.ValidateAsync(It.IsAny<CreateSubcategoryModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ValidationResult(new[] { validationFailure }));

            // Act
            var result = await _controller.CreateSubcategory(subcategory);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetSubcategories_ReturnsOkObjectResultWithCollectionOfSubcategories()
        {
            // Arrange
            _service.Setup(service => service.GetSubcategoriesAsync())
                .ReturnsAsync(GetTestSubcategories());

            // Act
            var result = await _controller.GetSubcategories();

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
            var subcategories = Assert.IsAssignableFrom<IEnumerable<Subcategory>>(objectResult.Value);
            Assert.Equal(GetTestSubcategories().Count(), subcategories.Count());
        }

        [Fact]
        public async Task GetSubcategory_HasInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var testSubcategoryId = Guid.Empty;

            // Act
            var result = await _controller.GetSubcategory(testSubcategoryId);

            // Assert
            var objectResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetSubcategory_HasValidId_ReturnsOkObjectResultWithSubcategory()
        {
            // Arrange
            var expectedSubcategory = GetTestSubcategories().FirstOrDefault();

            _service.Setup(service => service.GetSubcategoryByIdAsync(It.IsAny<Guid>()))
               .ReturnsAsync(expectedSubcategory);
            
            // Act
            var result = await _controller.GetSubcategory(Guid.NewGuid());

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
            var subcategory = Assert.IsType<Subcategory>(objectResult.Value);
            Assert.Equal(expectedSubcategory.Name, subcategory.Name);
            Assert.Equal(expectedSubcategory.CategoryId, subcategory.CategoryId);
            Assert.Equal(expectedSubcategory.Id, subcategory.Id);
        }

        private IEnumerable<Subcategory> GetTestSubcategories()
        {
            IEnumerable<Subcategory> subcategories = new List<Subcategory>
            {
                new Subcategory("Name1", Guid.NewGuid()),
                new Subcategory("Name2", Guid.NewGuid()),
                new Subcategory("Name3", Guid.NewGuid())
            };
            return subcategories;
        }
    }
}
