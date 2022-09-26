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
    public class LanguagesControllerTests
    {
        private readonly Mock<ILanguageService> _service;
        private readonly Mock<IValidator<CreateLanguageModel>> _createLanguageValidator;
        private readonly Mock<IValidator<EditLanguageModel>> _editLanguageValidator;
        private readonly LanguagesController _controller;

        public LanguagesControllerTests()
        {
            _service = new Mock<ILanguageService>();
            _createLanguageValidator = new Mock<IValidator<CreateLanguageModel>>();
            _editLanguageValidator = new Mock<IValidator<EditLanguageModel>>();

            _controller = new LanguagesController(_service.Object, _createLanguageValidator.Object, _editLanguageValidator.Object);
        }

        [Fact]
        public async Task CreateLanguage_WhenModelIsValid_ReturnsOkResult()
        {
            // Arrange
            var model = new CreateLanguageModel
            {
                Name = "Name",
                Code = "Code",
            };
            var validationResult = new ValidationResult();

            _createLanguageValidator.Setup(validator => validator.ValidateAsync(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateLanguage(model);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task CreateLanguage_WhenModelIsInvalid_ReturnsBadRequestResult()
        {
            // Arrange
            var model = new CreateLanguageModel
            {
                Name = string.Empty
            };

            var validationFailure = new ValidationFailure(nameof(model.Name), Errors.LanguageNameCannotBeEmpty);
            var validationResult = new ValidationResult(new[] { validationFailure });

            _createLanguageValidator.Setup(validator => validator.ValidateAsync(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.CreateLanguage(model);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(validationFailure.ErrorMessage, objectResult.Value);
        }

        [Fact]
        public async Task EditLanguage_WhenModelIsValid_ReturnsOkResult()
        {
            // Arrange
            var model = new EditLanguageModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Code = "Code",
                IsDefault = false,
                IsHidden = true,
                IsAdded = false,
            };
            var validationResult = new ValidationResult();

            _editLanguageValidator.Setup(validator => validator.ValidateAsync(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.EditLanguage(model);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);
        }

        [Fact]
        public async Task EditLanguage_WhenModelIsInvalid_ReturnsBadRequestResult()
        {
            // Arrange
            var model = new EditLanguageModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Code = "Code",
                IsDefault = false,
                IsHidden = true,
                IsAdded = false,
            };

            var validationFailure = new ValidationFailure(nameof(model.Name), Errors.LanguageNameCannotBeEmpty);
            var validationResult = new ValidationResult(new[] { validationFailure });

            _editLanguageValidator.Setup(validator => validator.ValidateAsync(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResult);

            // Act
            var result = await _controller.EditLanguage(model);

            // Assert
            var objectResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
            Assert.Equal(validationFailure.ErrorMessage, objectResult.Value);
        }

        [Fact]
        public async Task DeleteLanguage_WhenLanguageDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var languageId = Guid.NewGuid();

            _service.Setup(service => service.DeleteLanguageAsync(languageId))
                .ReturnsAsync((Language)null);

            // Act
            var result = await _controller.DeleteLanguage(languageId);

            // Assert
            var objectResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task DeleteLanguage_WhenLanguageExists_ReturnsOkObjectResultWithArticle()
        {
            // Arrange
            var expectedLanguageId = Guid.NewGuid();

            var expectedLanguage = new Language()
            {
                Id = expectedLanguageId,
                Name = "Name",
                Code = "Code",
                IsDefault = false,
                IsHidden = true,
                IsAdded = false
            };

            _service.Setup(service => service.DeleteLanguageAsync(expectedLanguageId))
               .ReturnsAsync(expectedLanguage);

            // Act
            var result = await _controller.DeleteLanguage(expectedLanguageId);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            var actualLanguage = Assert.IsType<Language>(objectResult.Value);
            Assert.Equal(expectedLanguage.Id, actualLanguage.Id);
            Assert.Equal(expectedLanguage.Name, actualLanguage.Name);
            Assert.Equal(expectedLanguage.Code, actualLanguage.Code);
            Assert.Equal(expectedLanguage.IsDefault, actualLanguage.IsDefault);
            Assert.Equal(expectedLanguage.IsHidden, actualLanguage.IsHidden);
            Assert.Equal(expectedLanguage.IsAdded, actualLanguage.IsAdded);
        }

        [Fact]
        public async Task GetLanguages_ReturnsOkObjectResultWithLanguages()
        {
            // Arrange
            var expectedLangugaes = GetLanguages();

            _service.Setup(service => service.GetLanguagesAsync())
                .ReturnsAsync(expectedLangugaes);

            // Act
            var result = await _controller.GetLanguages();

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, objectResult.StatusCode);

            var actualLanguages = Assert.IsAssignableFrom<IEnumerable<Language>>(objectResult.Value);
            Assert.Equal(expectedLangugaes, actualLanguages);
        }

        [Fact]
        public async Task GetLanguage_WhenLanguageDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var languageId = Guid.NewGuid();

            _service.Setup(service => service.GetLanguageByIdAsync(languageId))
                .ReturnsAsync((Language)null);

            // Act
            var result = await _controller.GetLanguage(languageId);

            // Assert
            var objectResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, objectResult.StatusCode);
        }

        [Fact]
        public async Task GetLanguage_WhenLanguageExists_ReturnsOkObjectResultWithLanguage()
        {
            // Arrange
            var expectedLanguageId = Guid.NewGuid();

            var expectedLanguage = new Language { Name = "Name", Code = "Code" };
            expectedLanguage.Id = expectedLanguageId;

            _service.Setup(service => service.GetLanguageByIdAsync(expectedLanguageId))
               .ReturnsAsync(expectedLanguage);

            // Act
            var result = await _controller.GetLanguage(expectedLanguageId);

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, objectResult.StatusCode);

            var actualLanguage = Assert.IsType<Language>(objectResult.Value);
            Assert.Equal(expectedLanguage.Name, actualLanguage.Name);
            Assert.Equal(expectedLanguage.Code, actualLanguage.Code);
            Assert.Equal(expectedLanguage.Id, actualLanguage.Id);
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