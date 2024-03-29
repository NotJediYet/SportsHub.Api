﻿using FluentValidation;
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
    public class ArticlesControllerTests
    {
        private readonly Mock<IArticleService> _serviceArticle;
        private readonly Mock<IValidator<CreateArticleModel>> _validatorArticle;
        private readonly ArticlesController _controller;

        public ArticlesControllerTests()
        {
            _serviceArticle = new Mock<IArticleService>();

            _validatorArticle = new Mock<IValidator<CreateArticleModel>>();

            _controller = new ArticlesController(_serviceArticle.Object, _validatorArticle.Object);
        }

        [Fact]
        public async Task CreateArticle_WhenModelIsValid_ReturnsOkResult()
        {
            // Arrange
            var articleModel = new CreateArticleModel
            {
                TeamId = Guid.NewGuid(),
                Location = "location",
                AltImage = "altImage",
                Headline = "headline",
                Caption = "caption",
                Content = "content",
                IsShowComments = false
            };
            var validationResult = new ValidationResult();

            _validatorArticle.Setup(validator => validator.ValidateAsync(articleModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateArticle(articleModel);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateArticle_WhenModelIsInvalid_ReturnsBadRequestResult()
        {
            // Arrange
            var articleModel = new CreateArticleModel
            {
                TeamId = Guid.NewGuid(),
                Location = "location",
                AltImage= "altImage",
                Headline = string.Empty,
                Caption = "caption",
                Content = "content",
                IsShowComments=false
            };

            var validationFailure = new ValidationFailure(nameof(articleModel.Headline), Errors.ArticleHeadlineCannotBeEmpty);
            var validationResult = new ValidationResult(new[] { validationFailure });

            _validatorArticle.Setup(validator => validator.ValidateAsync(articleModel, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateArticle(articleModel);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(validationFailure.ErrorMessage, objectResult.Value);
        }

        [Fact]
        public async Task GetArticles_ReturnsOkObjectResultWithArticles()
        {
            // Arrange
            var expectedArticles = GetArticles();

            _serviceArticle.Setup(service => service.GetArticlesAsync())
                .ReturnsAsync(expectedArticles);

            // Act
            var result = await _controller.GetArticles();

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            var actualArticles = Assert.IsAssignableFrom<IEnumerable<Article>>(objectResult.Value);
            Assert.Equal(expectedArticles, actualArticles);
        }

        [Fact]
        public async Task GetArticle_WhenArticleDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var articleId = Guid.NewGuid();

            _serviceArticle.Setup(service => service.GetArticleByIdAsync(articleId))
                .ReturnsAsync((Article)null);

            // Act
            var result = await _controller.GetArticle(articleId);

            // Assert
            var objectResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetArticle_WhenArticleExists_ReturnsOkObjectResultWithArticle()
        {
            // Arrange
            var expectedArticleId = Guid.NewGuid();

            var expectedArticle = new Article(teamId: Guid.NewGuid(), location: "location", altImage: "AltImage", headline: "headline", caption: "caption", content: "content", isShowComments: false);
            expectedArticle.Id = expectedArticleId;
          

            _serviceArticle.Setup(service => service.GetArticleByIdAsync(expectedArticleId))
               .ReturnsAsync(expectedArticle);

            // Act
            var result = await _controller.GetArticle(expectedArticleId);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            var actualArticle = Assert.IsType<Article>(objectResult.Value);
            Assert.Equal(expectedArticle.TeamId, actualArticle.TeamId);
            Assert.Equal(expectedArticle.Id, actualArticle.Id);
            Assert.Equal(expectedArticle.Location, actualArticle.Location);
            Assert.Equal(expectedArticle.Headline, actualArticle.Headline);
            Assert.Equal(expectedArticle.Caption, actualArticle.Caption);
        }

        private IEnumerable<Article> GetArticles()
        {
            IEnumerable<Article> articles = new List<Article>
            {
                new Article(Guid.NewGuid(),"location1" ,"altImage1","headline1" ,"caption1" ,"content1", false),
                new Article(Guid.NewGuid(),"location2" ,"altImage2","headline2" ,"caption2" ,"content2", true),
                new Article(Guid.NewGuid(),"location3" ,"altImage3","headline3" ,"caption3" ,"content3", false)
            };

            return articles;
        }

        [Fact]
        public async Task DeleteArticle_WhenArticleDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var articleId = Guid.NewGuid();

            _serviceArticle.Setup(service => service.DeleteArticleAsync(articleId))
                .ReturnsAsync((Article)null);

            // Act
            var result = await _controller.DeleteArticle(articleId);

            // Assert
            var objectResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteArticle_WhenArticleExists_ReturnsOkObjectResultWithArticle()
        {
            // Arrange
            var expectedArticleId = Guid.NewGuid();

            var expectedArticle = new Article(teamId: Guid.NewGuid(), location: "location", altImage: "AltImage", headline: "headline", caption: "caption", content: "content", isShowComments: false);
            expectedArticle.Id = expectedArticleId;

            _serviceArticle.Setup(service => service.DeleteArticleAsync(expectedArticleId))
               .ReturnsAsync(expectedArticle);

            // Act
            var result = await _controller.DeleteArticle(expectedArticleId);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            var actualArticle = Assert.IsType<Article>(objectResult.Value);
            Assert.Equal(expectedArticle.Headline, actualArticle.Headline);
            Assert.Equal(expectedArticle.Id, actualArticle.Id);
            Assert.Equal(expectedArticle.TeamId, actualArticle.TeamId);
        }
    }
}