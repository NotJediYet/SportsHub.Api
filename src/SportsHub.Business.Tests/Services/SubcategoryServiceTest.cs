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
    public class SubcategoryServiceTest
    {
        private readonly Mock<ISubcategoryRepository> _repository;
        private readonly ISubcategoryService _service;

        public SubcategoryServiceTest()
        {
            _repository = new Mock<ISubcategoryRepository>();
            _service = new SubcategoryService(_repository.Object);
        }

        [Fact]
        public async Task GetSubcategoriesAsync_ReturnsCollectionOfSubcategories()
        {
            // Arrange
            _repository.Setup(repo => repo.GetSubcategoriesAsync())
                .ReturnsAsync(GetTestSubcategories());

            // Act
            var result = await _service.GetSubcategoriesAsync();

            // Assert
            Assert.IsAssignableFrom<IEnumerable<Subcategory>>(result);
            Assert.Equal(GetTestSubcategories().Count(), result.Count());
        }

        [Fact]
        public async Task GetSubcategoryByIdAsync_HasValidId_ReturnsSubcategory()
        {
            // Arrange
            var testSubcategory = GetTestSubcategories().FirstOrDefault();

            _repository.Setup(repo => repo.GetSubcategoryByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(testSubcategory);

            // Act
            var result = await _service.GetSubcategoryByIdAsync(Guid.NewGuid());

            // Assert
            var subcategory = Assert.IsType<Subcategory>(result);
            Assert.Equal(testSubcategory.Name, subcategory.Name);
            Assert.Equal(testSubcategory.CategoryId, subcategory.CategoryId);
            Assert.Equal(testSubcategory.Id, subcategory.Id);
        }

        [Fact]
        public async Task CreateSubcategoryAsync_HasValidValues()
        {
            // Arrange
            var subcategoryName = "Name";

            // Act
            await _service.CreateSubcategoryAsync(subcategoryName, Guid.NewGuid());

            // Assert
            _repository.Verify(repo => repo.AddSubcategoryAsync(It.IsAny<Subcategory>()));
        }

        [Fact]
        public async Task DoesSubcategoryAlreadyExistByNameAsync_HasExistedName_ReturnsTrue()
        {
            // Arrange
            var sabcategoryName = GetTestSubcategories().FirstOrDefault().Name;

            _repository.Setup(repo => repo.DoesSubcategoryAlreadyExistByNameAsync(sabcategoryName))
                .ReturnsAsync(GetTestSubcategories().Any(subcategory => subcategory.Name == sabcategoryName));

            // Act
            var result = await _service.DoesSubcategoryAlreadyExistByNameAsync(sabcategoryName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesSubcategoryAlreadyExistByNameAsync_HasNotExistedName_ReturnsFalse()
        {
            // Arrange
            var subcategoryName = "Test unique name";

            _repository.Setup(repo => repo.DoesSubcategoryAlreadyExistByNameAsync(subcategoryName))
                .ReturnsAsync(GetTestSubcategories().Any(subcategory => subcategory.Name == subcategoryName));

            // Act
            var result = await _service.DoesSubcategoryAlreadyExistByNameAsync(subcategoryName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DoesSubcategoryAlredyExistByIdAsync_HasExistedId_ReturnsTrue()
        {
            // Arrange
            var subcategoryId = GetTestSubcategories().FirstOrDefault().Id;

            _repository.Setup(repo => repo.DoesSubcategoryAlredyExistByIdAsync(subcategoryId))
                .ReturnsAsync(GetTestSubcategories().Any(subcategory => subcategory.Id == subcategoryId));

            // Act
            var result = await _service.DoesSubcategoryAlredyExistByIdAsync(subcategoryId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesSubcategoryAlredyExistByIdAsync_HasNotExistedId_ReturnsFalse()
        {
            // Arrange
            Guid subcategoryId;

            do
            {
                subcategoryId = Guid.NewGuid();
            } while (GetTestSubcategories().Any(subcategory => subcategory.Id == subcategoryId));

            _repository.Setup(repo => repo.DoesSubcategoryAlredyExistByIdAsync(subcategoryId))
                .ReturnsAsync(GetTestSubcategories().Any(subcategory => subcategory.Id == subcategoryId));

            // Act
            var result = await _service.DoesSubcategoryAlredyExistByIdAsync(subcategoryId);

            // Assert
            Assert.False(result);
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
