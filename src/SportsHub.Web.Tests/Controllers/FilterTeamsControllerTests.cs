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
    public class FilterTeamsControllerTests
    {
        private readonly Mock<ICategoryService> _categoryService;
        private readonly Mock<ITeamService> _teamService;
        private readonly Mock<ISubcategoryService> _subcategoryService;
        private readonly FilterTeamsController _controller;

        public FilterTeamsControllerTests()
        {
            _categoryService = new Mock<ICategoryService>();

            _teamService = new Mock<ITeamService>();

            _subcategoryService = new Mock<ISubcategoryService>();

            _controller = new FilterTeamsController(_teamService.Object, _subcategoryService.Object, _categoryService.Object);
        }

        [Fact]

        public async Task GetFilteredTeams_WhenTeamsExists_ReturnsOkObjectResultWithTeams()
        {
            // Arrange
            var expectedCategoryName = "CategoryName";
            var expectedCategoryTaskId = Task.FromResult(Guid.NewGuid());
            var expectedCategoryId = Guid.NewGuid();
            var expectedSubcategoryId = Guid.NewGuid();
            var expectedSubcategoryIds = new List<Guid>
            {
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid()
            };
            var expectedTeamLocation = "USA";

            var expectedTeams = GetTeams(expectedSubcategoryId);

            _teamService.Setup(service => service.GetSortedTeamAsync())
                .ReturnsAsync(expectedTeams);

            _teamService.Setup(service => service.GetTeamsFilteredByLocation(expectedTeamLocation, expectedTeams))
                .Returns(expectedTeams);

            _teamService.Setup(service => service.GetTeamsFilteredBySubcategoryIds(expectedSubcategoryIds, expectedTeams))
                .Returns(expectedTeams);

            _teamService.Setup(service => service.GetTeamsFilteredBySubcategoryId(expectedSubcategoryId, expectedTeams))
                .Returns(expectedTeams);

            _categoryService.Setup(service => service.FindCategoryIdByCategoryNameAsync(expectedCategoryName))
                .Returns(expectedCategoryTaskId);

            // Act
            var result = await _controller.GetFilteredTeams("All", "All", "All");

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            var actualTeams = Assert.IsAssignableFrom<List<Team>>(objectResult.Value);
            Assert.Equal(expectedTeams, actualTeams);
        }

        private List<Team> GetTeams(Guid subcategoryId)
        {
            List<Team> teams = new List<Team>
            {
                new Team()
                {
                    Name = "teamName1",
                    SubcategoryId = subcategoryId,
                    Location = "location1"
                },
                new Team()
                {
                    Name = "teamName2",
                    SubcategoryId = subcategoryId,
                    Location = "location2"
                },
                new Team()
                {
                    Name = "teamName3",
                    SubcategoryId = subcategoryId,
                    Location = "location3"
                }
            };
            return teams;
        }
    }
}
