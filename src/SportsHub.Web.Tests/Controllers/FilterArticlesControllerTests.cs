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
    public class FilterArticlesControllerTests
    {
        private readonly Mock<IArticleService> _articleService;
        private readonly Mock<ITeamService> _teamService;
        private readonly Mock<ICategoryService> _categoryService;
        private readonly Mock<ISubcategoryService> _subcategoryService;
        private readonly FilterArticlesController _controller;

        public FilterArticlesControllerTests()
        {
            _articleService = new Mock<IArticleService>();

            _teamService = new Mock<ITeamService>();

            _categoryService = new Mock<ICategoryService>();

            _subcategoryService = new Mock<ISubcategoryService>();

            _controller = new FilterArticlesController(_articleService.Object, _teamService.Object, _subcategoryService.Object, _categoryService.Object);
        }

        [Fact]

        public async Task GetFilteredArticles_WhenArticlesExists_ReturnsOkObjectResultWithArticles()
        {
            // Arrange
            var expectedCategoryName = "CategoryName";
            var expectedCategoryId = Guid.NewGuid();
            var expectedSubcategoryName = "SubcategoryName";
            var expectedTeamName = "Team2";
            var expectedStatus = "Published";
            var expectedSubcategory = new Subcategory { Name = expectedSubcategoryName, CategoryId = expectedCategoryId };
            var expectedSubcategoryId = expectedSubcategory.Id;
            var expectedTeam = new Team
            { 
               Name = expectedTeamName, 
               SubcategoryId = expectedSubcategoryId 
            };
            var expectedTeamId = expectedTeam.Id;

            var expectedArticles = GetArticles(expectedTeamId);

            _articleService.Setup(service => service.GetArticlesAsync())
            .ReturnsAsync(expectedArticles);

            _categoryService.Setup(service => service.FindCategoryIdByCategoryNameAsync(expectedCategoryName))
           .ReturnsAsync(expectedCategoryId);

            _subcategoryService.Setup(service => service.FindSubcategoryIdBySubcategoryNameAsync(expectedSubcategoryName))
            .ReturnsAsync(expectedSubcategoryId);

            _subcategoryService.Setup(service => service.GetSubcategoryByIdAsync(expectedSubcategoryId))
            .ReturnsAsync(expectedSubcategory);

            _teamService.Setup(service => service.GetTeamIdByNameAsync(expectedTeamName))
             .ReturnsAsync(expectedTeamId);

            _teamService.Setup(service => service.GetTeamByIdAsync(expectedTeamId))
             .ReturnsAsync(expectedTeam);

            _articleService.Setup(service => service.GetArticlesFilteredByTeamId(expectedTeamId, expectedArticles))
            .Returns(expectedArticles);

            _articleService.Setup(service => service.GetArticlesFilteredByStatus(expectedStatus, expectedArticles))
            .Returns(expectedArticles);

           
            // Act
            var result = await _controller.GetFilteredArticles(expectedCategoryName, expectedSubcategoryName, expectedTeamName, expectedStatus);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            var actualArticles = Assert.IsAssignableFrom<IEnumerable<Article>>(objectResult.Value);
            Assert.Equal(expectedArticles, actualArticles);
        }

        private IEnumerable<Article> GetArticles(Guid teamId)
        {
            IEnumerable<Article> articles = new List<Article>
            {
                new Article(teamId,"location1" ,"altImage1","headline1" ,"caption1" ,"content1", true),
                new Article(teamId,"location2" ,"altImage2","headline2" ,"caption2" ,"content2", true),
                new Article(teamId,"location3" ,"altImage3","headline3" ,"caption3" ,"content3", true)
            };
            return articles;
        }
    }
}
