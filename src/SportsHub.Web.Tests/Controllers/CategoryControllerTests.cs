using Microsoft.AspNetCore.Mvc;
using Moq;
using SportsHub.Business.Services.Abstraction;
using SportsHub.Shared.Models;
using SportsHub.Web.Controllers;
using System.Threading.Tasks;
using Xunit;

namespace SportsHub.Web.Tests.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _service;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _service = new Mock<ICategoryService>();

            _controller = new CategoryController(_service.Object);
        }

        [Fact]
        public async Task CreateCategoryAsync_HasValidValues_ReturnsOkResult()
        {
            // Arrange
            var category = new CreateCategoryModel
            {
                Name = "Name",
            };

            // Act
            var result = await _controller.CreateCategoryAsync(category);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(200, objectResult.StatusCode);
        }

    }
}
