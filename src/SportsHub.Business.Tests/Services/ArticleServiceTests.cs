﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using SportsHub.Business.Repositories;
using SportsHub.Business.Services;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace SportsHub.Business.Tests.Services
{
    public class ArticleServiceTests
    {
        private readonly Mock<IArticleRepository> _articleRepository;
        private readonly IArticleService _service;
        private readonly Mock<IArticleImageRepository> _articleImageRepository;

        public ArticleServiceTests()
        {
            _articleRepository = new Mock<IArticleRepository>();
            _articleImageRepository = new Mock<IArticleImageRepository>();
            _service = new ArticleService(_articleRepository.Object ,_articleImageRepository.Object);
        }

        [Fact]
        public async Task GetArticlesAsync_ReturnsExpectedArticles()
        {
            // Arrange
            var expectedArticles = GetArticles();

            _articleRepository.Setup(repository => repository.GetArticlesAsync())
            .ReturnsAsync(expectedArticles);

            // Act
            var actualArticles = await _service.GetArticlesAsync();

            // Assert
            Assert.Equal(expectedArticles, actualArticles);
        }

        [Fact]
        public async Task GetArticleByIdAsync_WhenIdIsValid_ReturnsExpectedArticle()
        {
            // Arrange
            var expectedArticleId = Guid.NewGuid();

            var expectedArticle = new Article(teamId: Guid.NewGuid(), location: "location", altImage: "AltImage", headline: "headline", caption: "caption", content: "content", isShowComments: false);
            expectedArticle.Id = expectedArticleId;

            _articleRepository.Setup(repo => repo.GetArticleByIdAsync(expectedArticleId))
            .ReturnsAsync(expectedArticle);

            // Act
            var actualArticle = await _service.GetArticleByIdAsync(expectedArticleId);

            // Assert
            Assert.Equal(expectedArticle.Headline, actualArticle.Headline);
            Assert.Equal(expectedArticle.Id, actualArticle.Id);
            Assert.Equal(expectedArticle.TeamId, actualArticle.TeamId);
        }

        [Fact]
        public async Task CreateArticleAsync_CallsAppropriateRepositoryMethodWithParameters()
        {
            // Arrange
            var expectedTeamId = Guid.NewGuid();
            var expectedLocation = "location";
            var expectedAltImage = "altImage";
            var expectedHeadline = "headline";
            var expectedCaption = "caption";
            var expectedContent = "content";
            var expectedIsShowComments = true;

            CreateArticleModel articleModel = new()
            {
                TeamId = expectedTeamId,
                Location = expectedLocation,
                AltImage = expectedAltImage,
                Headline = expectedHeadline,
                Caption = expectedCaption,
                Content = expectedContent,
                IsShowComments = expectedIsShowComments
            };

            var imageStream = new MemoryStream(GetRandomBytes());

            IFormFile expectedArticleImage = new FormFile(imageStream, 0, imageStream.Length, "UnitTest", "UnitTest.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg",
            };

            // Act
            await _service.CreateArticleAsync(articleModel);

            // Assert
            _articleRepository.Verify(repository => repository.AddArticleAsync(It.Is<Article>(article =>
                    (article.TeamId == expectedTeamId) && (article.Location == expectedLocation) && (article.AltImage == expectedAltImage)
                && (article.Headline == expectedHeadline) && (article.Caption == expectedCaption) && (article.Content == expectedContent) &&
                article.IsShowComments == expectedIsShowComments))); ;
        }


        [Fact]
        public async Task DoesArticleAlreadyExistByHeadlineAsync_WhenArticleExists_ReturnsTrue()
        {
            // Arrange
            var articleHeadline = "Headline";

            _articleRepository.Setup(repository => repository.DoesArticleAlreadyExistByHeadlineAsync(articleHeadline))
            .ReturnsAsync(true);

            // Act
            var result = await _service.DoesArticleAlreadyExistByHeadlineAsync(articleHeadline);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesArticleAlreadyExistByHeadlineAsync_WhenArticleDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var articleHeadline = "Headline";

            _articleRepository.Setup(repository => repository.DoesArticleAlreadyExistByHeadlineAsync(articleHeadline))
            .ReturnsAsync(false);

            // Act
            var result = await _service.DoesArticleAlreadyExistByHeadlineAsync(articleHeadline);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DoesArticleAlredyExistByIdAsync_WhenArticleExists_ReturnsTrue()
        {
            // Arrange
            var subcategoryId = Guid.NewGuid();

            _articleRepository.Setup(repo => repo.DoesArticleAlreadyExistByIdAsync(subcategoryId))
            .ReturnsAsync(true);

            // Act
            var result = await _service.DoesArticleAlreadyExistByIdAsync(subcategoryId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesArticleAlredyExistByIdAsync_WhenArticleDoesNotExists_ReturnsFalse()
        {
            // Arrange
            var articleId = Guid.NewGuid();

            _articleRepository.Setup(repo => repo.DoesArticleAlreadyExistByIdAsync(articleId))
             .ReturnsAsync(false);

            // Act
            var result = await _service.DoesArticleAlreadyExistByIdAsync(articleId);

            // Assert
            Assert.False(result);
        }

        private IEnumerable<Article> GetArticles()
        {
            IEnumerable<Article> articles = new List<Article>
            {
                 new Article( Guid.NewGuid(),"location1" ,"altImage1","headline1" ,"caption1" ,"content1", false),
                new Article(Guid.NewGuid(),"location2" ,"altImage2","headline2" ,"caption2" ,"content2", true),
                new Article(Guid.NewGuid(),"location3" ,"altImage3","headline3" ,"caption3" ,"content3", false)
            };

            return articles;
        }

        public byte[] GetRandomBytes()
        {
            Random random = new Random();
            int randomNumber = random.Next(0, 100);
            List<byte> bytes = new List<byte>();
            for (int b = 0; b < randomNumber; b++)
                bytes.Add((byte)b);
            return bytes.ToArray();
        }
    }
}