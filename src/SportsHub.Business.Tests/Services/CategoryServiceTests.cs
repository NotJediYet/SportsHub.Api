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

            var expectedCategory = new Category { Name = "Name" };
            expectedCategory.Id = expectedCategoryId;

            _repository.Setup(repository => repository.GetCategoryByIdAsync(expectedCategoryId))
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

            _repository.Setup(repository => repository.DoesCategoryAlreadyExistByIdAsync(categoryId))
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

            _repository.Setup(repository => repository.DoesCategoryAlreadyExistByIdAsync(categoryId))
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
                new Category { Name = "Name1" },
                new Category { Name = "Name2" },
                new Category { Name = "Name3" },
            };

            return categories;
        }

        [Fact]
        public async Task EditCategoryAsync_CallsAppropriateRepositoryMethodWithParameters()
        {
            // Arrange
            var expectedCategoryId = Guid.NewGuid();
            var expectedCategoryName = "Name";
            var expectedIsStatic = false;
            var expectedCategoryIsHidden = true;
            var expectedCategoryOrderIndex = 1;

            EditCategoryModel categoryModel = new()
            {
                Id = expectedCategoryId,
                Name = expectedCategoryName,
                IsStatic = expectedIsStatic,
                IsHidden = expectedCategoryIsHidden,
                OrderIndex = expectedCategoryOrderIndex
            };

            // Act
            await _service.EditCategoryAsync(categoryModel);

            // Assert
            _repository.Verify(repository => repository.EditCategoryAsync(It.Is<Category>(category =>
                (category.Id == expectedCategoryId)
                && (category.Name == expectedCategoryName)
                && (category.IsStatic == expectedIsStatic)
                && (category.IsHidden == expectedCategoryIsHidden)
                && (category.OrderIndex == expectedCategoryOrderIndex))));
        }

        [Fact]
        public async Task DeleteCategoryAsync_WhenIdIsValid_ReturnsExpectedSubcategory()
        {
            // Arrange
            var expectedCategoryId = Guid.NewGuid();

            var expectedCategory = new Category { Name = "Name" };
            expectedCategory.Id = expectedCategoryId;

            _repository.Setup(repository => repository.DeleteCategoryAsync(expectedCategoryId))
                .ReturnsAsync(expectedCategory);

            // Act
            var actualCategory = await _service.DeleteCategoryAsync(expectedCategoryId);

            // Assert
            Assert.Equal(expectedCategory.Name, actualCategory.Name);
        }
    }
}
