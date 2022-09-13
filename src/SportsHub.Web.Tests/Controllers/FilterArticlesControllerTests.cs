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
        private readonly Mock<ISubcategoryService> _subcategoryService;
        private readonly FilterArticlesController _controller;

        public FilterArticlesControllerTests()
        {
            _articleService = new Mock<IArticleService>();

            _teamService = new Mock<ITeamService>();

            _subcategoryService = new Mock<ISubcategoryService>();

            _controller = new FilterArticlesController(_articleService.Object, _teamService.Object, _subcategoryService.Object);
        }

        [Fact]

        public async Task GetFilteredArticles_WhenArticlesExists_ReturnsOkObjectResultWithArticles()
        {
            // Arrange
            var expectedSubcategoryName = "SubcategoryName";
            var expectedTeamName = "Team2";
            var expectedStatus = "Published";

            var expectedSubcategory = new Subcategory(expectedSubcategoryName, Guid.NewGuid());
            var expectedSubcategoryId = expectedSubcategory.Id;
            var expectedTeam = new Team
            { 
               Name = expectedTeamName, 
               SubcategoryId = expectedSubcategoryId 
            };
            var expectedTeamId = expectedTeam.Id;

            var expectedArticles = GetArticles(expectedTeamId);

            _articleService.Setup(service => service.GetSortedArticlesAsync())
            .ReturnsAsync(expectedArticles);

            _teamService.Setup(service => service.GetTeamIdByNameAsync(expectedTeamName))
             .ReturnsAsync(expectedTeamId);

            _articleService.Setup(service => service.GetArticlesFilteredByTeamId(expectedTeamId, expectedArticles))
            .Returns(expectedArticles);

            _articleService.Setup(service => service.GetArticlesFilteredByStatus(expectedStatus, expectedArticles))
            .Returns(expectedArticles);

            _subcategoryService.Setup(service => service.FindSubcategoryIdBySubcategoryNameAsync(expectedSubcategoryName))
            .ReturnsAsync(expectedSubcategoryId);

            _teamService.Setup(service => service.FindTeamIdBySubcategoryIdAsync(expectedSubcategoryId))
            .ReturnsAsync(expectedTeamId);

            // Act
            var result = await _controller.GetFilteredArticles(expectedSubcategoryName, expectedTeamName, expectedStatus);

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
