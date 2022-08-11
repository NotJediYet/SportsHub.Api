using Moq;
using SportsHub.Business.Repositories;
using SportsHub.Business.Services;
using SportsHub.Shared.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SportsHub.Business.Tests.Services
{
    public class CategoryServiceTest
    {
        private readonly Mock<ICategoryRepository> _repository;
        private readonly ICategoryService _service;

        public CategoryServiceTest()
        {
            _repository = new Mock<ICategoryRepository>();
            _service = new CategoryService(_repository.Object);
        }

        [Fact]
        public async Task GetCategoriesAsync_ReturnsCollectionOfCategories()
        {
            // Arrange
            _repository.Setup(repo => repo.GetCategoriesAsync())
                .ReturnsAsync(GetTestCategories());

            // Act
            var result = await _service.GetCategoriesAsync();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Category>>(result);
            Assert.Equal(GetTestCategories().Count(), result.Count());
        }

        [Fact]
        public async Task GetCategoryByIdAsync_HasValidId_ReturnsCategory()
        {
            // Arrange
            var testCategory = GetTestCategories().FirstOrDefault();

            _repository.Setup(repo => repo.GetCategoryByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(testCategory);

            // Act
            var result = await _service.GetCategoryByIdAsync(Guid.NewGuid());

            // Assert
            var category = Assert.IsType<Category>(result);
            Assert.Equal(testCategory.Name, category.Name);
            Assert.Equal(testCategory.Id, category.Id);
        }

        [Fact]
        public async Task CreateCategoryAsync_HasValidValues()
        {
            // Arrange
            var categoryName = "Name";

            // Act
            await _service.CreateCategoryAsync(categoryName);

            // Assert
            _repository.Verify(repo => repo.AddCategoryAsync(It.IsAny<Category>()));
        }
        
        [Fact]
        public async Task DoesCategoryAlreadyExistByNameAsync_HasExistedName_ReturnsTrue()
        {
            // Arrange
            var categoryName = GetTestCategories().FirstOrDefault().Name;

            _repository.Setup(repo => repo.DoesCategoryAlreadyExistByNameAsync(categoryName))
                .ReturnsAsync(GetTestCategories().Any(category => category.Name == categoryName));

            // Act
            var result = await _service.DoesCategoryAlreadyExistByNameAsync(categoryName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesCategoryAlreadyExistByNameAsync_HasNotExistedName_ReturnsFalse()
        {
            // Arrange
            var categoryName = "Test unique name";

            _repository.Setup(repo => repo.DoesCategoryAlreadyExistByNameAsync(categoryName))
                .ReturnsAsync(GetTestCategories().Any(category => category.Name == categoryName));

            // Act
            var result = await _service.DoesCategoryAlreadyExistByNameAsync(categoryName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DoesCategoryAlredyExistByIdAsync_HasExistedId_ReturnsTrue()
        {
            // Arrange
            var categoryId = GetTestCategories().FirstOrDefault().Id;

            _repository.Setup(repo => repo.DoesCategoryAlredyExistByIdAsync(categoryId))
                .ReturnsAsync(GetTestCategories().Any(category => category.Id == categoryId));

            // Act
            var result = await _service.DoesCategoryAlredyExistByIdAsync(categoryId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesCategoryAlredyExistByIdAsync_HasNotExistedId_ReturnsFalse()
        {
            // Arrange
            Guid categoryId;

            do
            {
                categoryId = Guid.NewGuid();
            } while (GetTestCategories().Any(category => category.Id == categoryId));

            _repository.Setup(repo => repo.DoesCategoryAlredyExistByIdAsync(categoryId))
                .ReturnsAsync(GetTestCategories().Any(category => category.Id == categoryId));

            // Act
            var result = await _service.DoesCategoryAlredyExistByIdAsync(categoryId);

            // Assert
            Assert.False(result);
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
