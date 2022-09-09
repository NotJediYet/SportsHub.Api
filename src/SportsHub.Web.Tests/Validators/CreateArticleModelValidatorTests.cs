using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;
using SportsHub.Web.Validators;
using System;
using Xunit;

namespace SportsHub.Web.Tests.Validators
{
    public class CreateArticleModelValidatorTests
    {
        private readonly Mock<ITeamService> _teamService;
        private readonly Mock<IArticleService> _articleService;
        private readonly CreateArticleModelValidator _validator;

        public CreateArticleModelValidatorTests()
        {
            _teamService = new Mock<ITeamService>();
            _articleService = new Mock<IArticleService>();
            _validator = new CreateArticleModelValidator(_teamService.Object, _articleService.Object);
        }

        [Fact]
        public async void CreateArticleModel_WhenNameIsEmpty_ReturnsValidationResultWithError()
        {
            // Arrange
            var article = new CreateArticleModel
            {
                TeamId = Guid.NewGuid(),
                Location = "location",
                AltImage = "altImage",
                Headline = string.Empty,
                Caption = "caption",
                Content = "content",
                IsShowComments = true,
            };

            var expectedErrorMessage = Errors.ArticleHeadlineCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(article);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateArticleModel_WhenNameIsNotUnique_ReturnsValidationResultWithError()
        {
            // Arrange
            var article = new CreateArticleModel
            {
                TeamId = Guid.NewGuid(),
                Location = "location",
                AltImage = "altImage",
                Headline = "headline",
                Caption = "caption",
                Content = "content",
                IsShowComments = true,
            };

            var expectedErrorMessage = Errors.ArticleHeadlineIsNotUnique;

            _articleService.Setup(service => service.DoesArticleAlreadyExistByHeadlineAsync(article.Headline))
            .ReturnsAsync(true);

            // Act
            var result = await _validator.ValidateAsync(article);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateArticleModel_WhenTeamIdIsEmpty_ReturnsValidationResultWithError()
        {
            // Arrange
            var article = new CreateArticleModel
            {
                TeamId = Guid.Empty,
                Location = "location",
                AltImage = "altImage",
                Headline = "headline",
                Caption = "caption",
                Content = "content",
                IsShowComments = true,
            };

            var expectedErrorMessage = Errors.TeamIdCanNotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(article);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateArticleModel_WhenTeamDoesNotExist_ReturnsValidationResultWithError()
        {
            // Arrange
            var article = new CreateArticleModel
            {
                TeamId = Guid.NewGuid(),
                Location = "location",
                AltImage = "altImage",
                Headline = "headline",
                Caption = "caption",
                Content = "content",
                IsShowComments = true,
            };

            var expectedErrorMessage = Errors.TeamIdDoesNotExist;

            _teamService.Setup(service => service.DoesTeamAlreadyExistByIdAsync(article.TeamId))
            .ReturnsAsync(false);
            // Act
            var result = await _validator.ValidateAsync(article);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateArticleModel_WhenModelIsValid_ReturnsSuccessValidationResult()
        {
            // Arrange
            var article = new CreateArticleModel
            {
                TeamId = Guid.NewGuid(),
                Location = "location",
                AltImage = "altImage",
                Headline = "headline",
                Caption = "caption",
                Content = "content",
                IsShowComments = true,
            };
        

            _articleService.Setup(service => service.DoesArticleAlreadyExistByHeadlineAsync(article.Headline))
            .ReturnsAsync(false);
            _teamService.Setup(service => service.DoesTeamAlreadyExistByIdAsync(article.TeamId))
            .ReturnsAsync(true);

            // Act
            var result = await _validator.ValidateAsync(article);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}