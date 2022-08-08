using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Web.Controllers;
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
        public async Task CreateCategoryAsync_HasValidValues_ReturnsOkResult()
        {
            // Arrange
            var category = new CreateCategoryModel
            {
                Name = "Name",
            };

            _validator.Setup(validator => validator.ValidateAsync(It.IsAny<CreateCategoryModel>(), It.IsAny<CancellationToken>()))
            .Returns(Task.Run(() => new ValidationResult()));

            // Act
            var result = await _controller.CreateCategory(category);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
        }

    }
}
