using Moq;
using SportsHub.Business.Repositories;
using SportsHub.Business.Services;
using SportsHub.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SportsHub.Business.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _repository;
        private readonly ICategoryService _service;

        public CategoryServiceTests()
        {
            _repository = new Mock<ICategoryRepository>();
            _service = new CategoryService(_repository.Object);
        }

        [Fact]
        public async Task GetCategoriesAsync_ReturnsExpectedCategories()
        {
            // Arrange
            var expectedCategories = GetCategories();

            _repository.Setup(repository => repository.GetCategoriesAsync())
                .ReturnsAsync(expectedCategories);

            // Act
            var actualCategories = await _service.GetCategoriesAsync();

            // Assert
            Assert.Equal(expectedCategories, actualCategories);
        }

        [Fact]
        public async Task GetCategoryByIdAsync_WhenIdIsValid_ReturnsExpectedCategory()
        {
            // Arrange
            var expectedCategoryId = Guid.NewGuid();

            var expectedCategory = new Category(name: "Name");
            expectedCategory.Id = expectedCategoryId;

            _repository.Setup(repo => repo.GetCategoryByIdAsync(expectedCategoryId))
                .ReturnsAsync(expectedCategory);

            // Act
            var actualCategory = await _service.GetCategoryByIdAsync(expectedCategoryId);

            // Assert
            Assert.Equal(expectedCategory.Name, actualCategory.Name);
            Assert.Equal(expectedCategory.Id, actualCategory.Id);
        }

        [Fact]
        public async Task CreateCategoryAsync_CallsAppropriateRepositoryMethodWithParameters()
        {
            // Arrange
            var expectedCategoryName = "Name";

            // Act
            await _service.CreateCategoryAsync(expectedCategoryName);

            // Assert
            _repository.Verify(repository => repository.AddCategoryAsync(It.Is<Category>(category => category.Name == expectedCategoryName)));
        }
        
        [Fact]
        public async Task DoesCategoryAlreadyExistByNameAsync_WhenCategoryExists_ReturnsTrue()
        {
            // Arrange
            var categoryName = "Name";

            _repository.Setup(repository => repository.DoesCategoryAlreadyExistByNameAsync(categoryName))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DoesCategoryAlreadyExistByNameAsync(categoryName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesCategoryAlreadyExistByNameAsync_WhenCategoryDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var categoryName = "Name";

            _repository.Setup(repository => repository.DoesCategoryAlreadyExistByNameAsync(categoryName))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DoesCategoryAlreadyExistByNameAsync(categoryName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DoesCategoryAlreadyExistByIdAsync_WhenCategoryExists_ReturnsTrue()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            _repository.Setup(repo => repo.DoesCategoryAlreadyExistByIdAsync(categoryId))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DoesCategoryAlreadyExistByIdAsync(categoryId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesCategoryAlreadyExistByIdAsync_WhenCategoryDoesNotExists_ReturnsFalse()
        {
            // Arrange
            var categoryId = Guid.NewGuid();

            _repository.Setup(repo => repo.DoesCategoryAlreadyExistByIdAsync(categoryId))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DoesCategoryAlreadyExistByIdAsync(categoryId);

            // Assert
            Assert.False(result);
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
