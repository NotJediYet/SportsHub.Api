﻿using Moq;
using SportsHub.Business.Repositories;
using SportsHub.Business.Services;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SportsHub.Business.Tests.Services
{
    public class SubcategoryServiceTests
    {
        private readonly Mock<ISubcategoryRepository> _repository;
        private readonly ISubcategoryService _service;

        public SubcategoryServiceTests()
        {
            _repository = new Mock<ISubcategoryRepository>();
            _service = new SubcategoryService(_repository.Object);
        }

        [Fact]
        public async Task GetSubcategoriesAsync_ReturnsExpectedSubcategories()
        {
            // Arrange
            var expectedSubcategories = GetSubcategories();

            _repository.Setup(repository => repository.GetSubcategoriesAsync())
                .ReturnsAsync(expectedSubcategories);

            // Act
            var actualSubcategories = await _service.GetSubcategoriesAsync();

            // Assert
            Assert.Equal(expectedSubcategories, actualSubcategories);
        }

        [Fact]
        public async Task GetSubcategoryByIdAsync_WhenIdIsValid_ReturnsExpectedSubcategory()
        {
            // Arrange
            var expectedSubcategoryId = Guid.NewGuid();

            var expectedSubcategory = new Subcategory { Name = "Name", CategoryId = Guid.NewGuid() };
            expectedSubcategory.Id = expectedSubcategoryId;

            _repository.Setup(repository => repository.GetSubcategoryByIdAsync(expectedSubcategoryId))
                .ReturnsAsync(expectedSubcategory);

            // Act
            var actualSubcategory = await _service.GetSubcategoryByIdAsync(expectedSubcategoryId);

            // Assert
            Assert.Equal(expectedSubcategory.Name, actualSubcategory.Name);
            Assert.Equal(expectedSubcategory.Id, actualSubcategory.Id);
            Assert.Equal(expectedSubcategory.CategoryId, actualSubcategory.CategoryId);
        }

        [Fact]
        public async Task CreateSubcategoryAsync_CallsAppropriateRepositoryMethodWithParameters()
        {
            // Arrange
            var expectedSubcategoryName = "Name";
            var expectedCategoryId = Guid.NewGuid();

            // Act
            await _service.CreateSubcategoryAsync(expectedSubcategoryName, expectedCategoryId);

            // Assert
            _repository.Verify(repository => repository.AddSubcategoryAsync(It.Is<Subcategory>(subcategory => 
                (subcategory.Name == expectedSubcategoryName) && (subcategory.CategoryId == expectedCategoryId))));
        }

        [Fact]
        public async Task DoesSubcategoryAlreadyExistByNameAsync_WhenSubcategoryExists_ReturnsTrue()
        {
            // Arrange
            var subcategoryName = "Name";

            _repository.Setup(repository => repository.DoesSubcategoryAlreadyExistByNameAsync(subcategoryName))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DoesSubcategoryAlreadyExistByNameAsync(subcategoryName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesSubcategoryAlreadyExistByNameAsync_WhenSubcategoryDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var subcategoryName = "Name";

            _repository.Setup(repository => repository.DoesSubcategoryAlreadyExistByNameAsync(subcategoryName))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DoesSubcategoryAlreadyExistByNameAsync(subcategoryName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DoesSubcategoryAlreadyExistByIdAsync_WhenSubcategoryExists_ReturnsTrue()
        {
            // Arrange
            var subcategoryId = Guid.NewGuid();

            _repository.Setup(repository => repository.DoesSubcategoryAlreadyExistByIdAsync(subcategoryId))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DoesSubcategoryAlreadyExistByIdAsync(subcategoryId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesSubcategoryAlreadyExistByIdAsync_WhenSubcategoryDoesNotExists_ReturnsFalse()
        {
            // Arrange
            var subcategoryId = Guid.NewGuid();

            _repository.Setup(repository => repository.DoesSubcategoryAlreadyExistByIdAsync(subcategoryId))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DoesSubcategoryAlreadyExistByIdAsync(subcategoryId);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public async Task FindSubcategoryIdBySubcategoryNameAsync_ReturnsExpectedSubcategoryId()
        {
            // Arrange
            var expectedSubcategoryName = "Name";
            var expectedSubcategoryId = Guid.NewGuid();

            _repository.Setup(repository => repository.FindSubcategoryIdBySubcategoryNameAsync(expectedSubcategoryName))
                .ReturnsAsync(expectedSubcategoryId);

            // Act
            var actualSubcategoryId = await _service.FindSubcategoryIdBySubcategoryNameAsync(expectedSubcategoryName);

            // Assert
            Assert.Equal(expectedSubcategoryId, actualSubcategoryId);
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
        public async Task EditSubcategoryAsync_CallsAppropriateRepositoryMethodWithParameters()
        {
            // Arrange
            var expectedSubcategoryId = Guid.NewGuid();
            var expectedSubcategoryName = "Name";
            var expectedCategoryId = Guid.NewGuid();
            var expectedSubcategoryIsHidden = true;
            var expectedSubcategoryOrderIndex = 1;

            EditSubcategoryModel subcategoryModel = new()
            {
                Id = expectedSubcategoryId,
                Name = expectedSubcategoryName,
                CategoryId = expectedCategoryId,
                IsHidden = expectedSubcategoryIsHidden,
                OrderIndex = expectedSubcategoryOrderIndex
            };

            // Act
            await _service.EditSubcategoryAsync(subcategoryModel);

            // Assert
            _repository.Verify(repository => repository.EditSubcategoryAsync(It.Is<Subcategory>(subcategory =>
                (subcategory.Id == expectedSubcategoryId)
                && (subcategory.Name == expectedSubcategoryName)
                && (subcategory.CategoryId == expectedCategoryId)
                && (subcategory.IsHidden == expectedSubcategoryIsHidden)
                && (subcategory.OrderIndex == expectedSubcategoryOrderIndex))));
        }

        [Fact]
        public async Task DeleteSubcategoryAsync_WhenIdIsValid_ReturnsExpectedSubcategory()
        {
            // Arrange
            var expectedSubcategoryId = Guid.NewGuid();

            var expectedSubcategory = new Subcategory { Name = "Name" };
            expectedSubcategory.Id = expectedSubcategoryId;

            _repository.Setup(repository => repository.DeleteSubcategoryAsync(expectedSubcategoryId))
                .ReturnsAsync(expectedSubcategory);

            // Act
            var actualSubcategory = await _service.DeleteSubcategoryAsync(expectedSubcategoryId);

            // Assert
            Assert.Equal(expectedSubcategory.Name, actualSubcategory.Name);
        }
    }
}
