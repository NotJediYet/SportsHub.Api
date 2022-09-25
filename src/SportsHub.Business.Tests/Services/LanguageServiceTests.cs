using Moq;
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
    public class LanguageServiceTests
    {
        private readonly Mock<ILanguageRepository> _repository;
        private readonly ILanguageService _service;

        public LanguageServiceTests()
        {
            _repository = new Mock<ILanguageRepository>();
            _service = new LanguageService(_repository.Object);
        }

        [Fact]
        public async Task GetLanguagesAsync_ReturnsExpectedLanguages()
        {
            // Arrange
            var expectedLanguages = GetLanguages();

            _repository.Setup(repository => repository.GetLanguagesAsync())
                .ReturnsAsync(expectedLanguages);

            // Act
            var actualLanguages = await _service.GetLanguagesAsync();

            // Assert
            Assert.Equal(expectedLanguages, actualLanguages);
        }

        [Fact]
        public async Task GetLanguageByIdAsync_WhenIdIsValid_ReturnsExpectedLanguage()
        {
            // Arrange
            var expectedLanguageId = Guid.NewGuid();

            var expectedLanguage = new Language { Name = "Name", Code = "Code" };
            expectedLanguage.Id = expectedLanguageId;

            _repository.Setup(repository => repository.GetLanguageByIdAsync(expectedLanguageId))
                .ReturnsAsync(expectedLanguage);

            // Act
            var actualLanguage = await _service.GetLanguageByIdAsync(expectedLanguageId);

            // Assert
            Assert.Equal(expectedLanguage.Name, actualLanguage.Name);
            Assert.Equal(expectedLanguage.Code, actualLanguage.Code);
            Assert.Equal(expectedLanguage.Id, actualLanguage.Id);
        }

        [Fact]
        public async Task CreateLanguageAsync_CallsAppropriateRepositoryMethodWithParameters()
        {
            // Arrange
            var expectedLanguageName = "Name";
            var expectedLanguageCode = "Code";
            var languageModel = new CreateLanguageModel{ Name = expectedLanguageName, Code = expectedLanguageCode };

            // Act
            await _service.CreateLanguageAsync(languageModel);

            // Assert
            _repository.Verify(repository => repository.AddLanguageAsync(It.Is<Language>(language => language.Name == expectedLanguageName && language.Code == expectedLanguageCode)));
        }

        [Fact]
        public async Task EditLanguageAsync_CallsAppropriateRepositoryMethodWithParameters()
        {
            // Arrange
            var expectedLanguageId = Guid.NewGuid();
            var expectedLanguageName = "Name";
            var expectedLanguageCode = "Code";
            var expectedIsDefault = false;
            var expectedIsHidden = true;
            var expectedIsAdded = false;

            EditLanguageModel languageModel = new()
            {
                Id = expectedLanguageId,
                Name = expectedLanguageName,
                Code = expectedLanguageCode,
                IsDefault = expectedIsDefault,
                IsHidden = expectedIsHidden,
                IsAdded = expectedIsAdded
            };

            // Act
            await _service.EditLanguageAsync(languageModel);

            // Assert
            _repository.Verify(repository => repository.EditLanguageAsync(It.Is<Language>(language =>
                (language.Id == expectedLanguageId)
                && (language.Name == expectedLanguageName)
                && (language.Code == expectedLanguageCode)
                && (language.IsDefault == expectedIsDefault)
                && (language.IsHidden == expectedIsHidden)
                && (language.IsAdded == expectedIsAdded))));
        }

        [Fact]
        public async Task DeleteLanguageAsync_WhenIdIsValid_ReturnsExpectedLanguage()
        {
            // Arrange
            var expectedLanguageId = Guid.NewGuid();
            var expectedLanguageName = "Name";
            var expectedLanguageCode = "Code";
            var expectedIsDefault = false;
            var expectedIsHidden = true;
            var expectedIsAdded = false;

            Language expectedLanguage = new()
            {
                Id = expectedLanguageId,
                Name = expectedLanguageName,
                Code = expectedLanguageCode,
                IsDefault = expectedIsDefault,
                IsHidden = expectedIsHidden,
                IsAdded = expectedIsAdded
            };

            _repository.Setup(repository => repository.DeleteLanguageAsync(expectedLanguageId))
                .ReturnsAsync(expectedLanguage);

            // Act
            var actualLanguage = await _service.DeleteLanguageAsync(expectedLanguageId);

            // Assert
            Assert.Equal(expectedLanguage.Id, actualLanguage.Id);
            Assert.Equal(expectedLanguage.Name, actualLanguage.Name);
            Assert.Equal(expectedLanguage.Code, actualLanguage.Code);
            Assert.Equal(expectedLanguage.IsDefault, actualLanguage.IsDefault);
            Assert.Equal(expectedLanguage.IsHidden, actualLanguage.IsHidden);
            Assert.Equal(expectedLanguage.IsAdded, actualLanguage.IsAdded);
        }

        [Fact]
        public async Task DoesLanguageAlreadyExistByNameAsync_WhenLanguageExists_ReturnsTrue()
        {
            // Arrange
            var languageName = "Name";

            _repository.Setup(repository => repository.DoesLanguageAlreadyExistByNameAsync(languageName))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DoesLanguageAlreadyExistByNameAsync(languageName);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesLanguageAlreadyExistByNameAsync_WhenLanguageDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var languageName = "Name";

            _repository.Setup(repository => repository.DoesLanguageAlreadyExistByNameAsync(languageName))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DoesLanguageAlreadyExistByNameAsync(languageName);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DoesLanguageAlreadyExistByCodeAsync_WhenLanguageExists_ReturnsTrue()
        {
            // Arrange
            var languageCode = "Code";

            _repository.Setup(repository => repository.DoesLanguageAlreadyExistByCodeAsync(languageCode))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DoesLanguageAlreadyExistByCodeAsync(languageCode);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesLanguageAlreadyExistByCodeAsync_WhenLanguageDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var languageCode = "Code";

            _repository.Setup(repository => repository.DoesLanguageAlreadyExistByCodeAsync(languageCode))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DoesLanguageAlreadyExistByCodeAsync(languageCode);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DoesLanguageAlreadyExistByIdAsync_WhenLanguageExists_ReturnsTrue()
        {
            // Arrange
            var languageId = Guid.NewGuid();

            _repository.Setup(repository => repository.DoesLanguageAlreadyExistByIdAsync(languageId))
                .ReturnsAsync(true);

            // Act
            var result = await _service.DoesLanguageAlreadyExistByIdAsync(languageId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoesLanguageAlreadyExistByIdAsync_WhenLanguageDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var languageId = Guid.NewGuid();

            _repository.Setup(repository => repository.DoesLanguageAlreadyExistByIdAsync(languageId))
                .ReturnsAsync(false);

            // Act
            var result = await _service.DoesLanguageAlreadyExistByIdAsync(languageId);

            // Assert
            Assert.False(result);
        }

        private IEnumerable<Language> GetLanguages()
        {
            IEnumerable<Language> languages = new List<Language>
            {
                new Language { Name = "Name1", Code = "Code1" },
                new Language { Name = "Name2", Code = "Code2" },
                new Language { Name = "Name3", Code = "Code3" }
            };

            return languages;
        }
    }
}
