using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using SportsHub.Business.Repositories;
using SportsHub.Business.Services;
using SportsHub.Shared.Entities;
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

            var expectedArticle = new Article(Guid.NewGuid(),location:"Location",headline: "Headline", caption: "Caption", context: "Context");
            expectedArticle.Id = expectedArticleId;

            _articleRepository.Setup(repository => repository.GetArticleByIdAsync(expectedArticleId))
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
            var expectedLocation ="location" ;
            var expectedHeadline = "headline";
            var expectedCaption = "caption";
            var expectedContext = "context";

            Article article = new Article(
                teamId: expectedTeamId, 
                location: expectedLocation,
                headline: expectedHeadline,
                caption: expectedCaption,
                context: expectedContext);

            var imageStream = new MemoryStream(GetRandomBytes());

            IFormFile expectedArticleImage = new FormFile(imageStream, 0, imageStream.Length, "UnitTest", "UnitTest.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg",
            };

            // Act
            await _service.CreateArticleAsync(article,expectedArticleImage);

            // Assert
            _articleRepository.Verify(repository => repository.AddArticleAsync(It.Is<Article>(article =>
                (article.TeamId == expectedTeamId) && (article.Location == expectedLocation)&&(article.Headline == expectedHeadline)
                &&(article.Caption == expectedCaption) && (article.Context == expectedContext))));
        }

        [Fact]
        public async Task DeleteArticleAsync_WhenIdIsValid_ReturnsExpectedArticle()
        {
            // Arrange
            var expectedArticleId = Guid.NewGuid();

            var expectedArticle = new Article(Guid.NewGuid(), location: "Location", headline: "Headline", caption: "Caption", context: "Context");
            expectedArticle.Id = expectedArticleId;

            _articleRepository.Setup(repository => repository.DeleteArticleAsync(expectedArticleId))
                .ReturnsAsync(expectedArticle);

            // Act
            var actualArticle = await _service.DeleteArticleAsync(expectedArticleId);

            // Assert
            Assert.Equal(expectedArticle.TeamId, actualArticle.TeamId);
            Assert.Equal(expectedArticle.Id, actualArticle.Id);
            Assert.Equal(expectedArticle.Location, actualArticle.Location);
            Assert.Equal(expectedArticle.Headline, actualArticle.Headline);
            Assert.Equal(expectedArticle.Caption, actualArticle.Caption);
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

            _articleRepository.Setup(repository => repository.DoesArticleAlreadyExistByIdAsync(subcategoryId))
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

            _articleRepository.Setup(repository => repository.DoesArticleAlreadyExistByIdAsync(articleId))
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
                new Article( Guid.NewGuid(),"location1" ,"headline1" ,"caption1" ,"context1"),
                new Article(Guid.NewGuid(),"location2" ,"headline2" ,"caption2" ,"context2"),
                new Article(Guid.NewGuid(),"location3" ,"headline3" ,"caption3" ,"context3")
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