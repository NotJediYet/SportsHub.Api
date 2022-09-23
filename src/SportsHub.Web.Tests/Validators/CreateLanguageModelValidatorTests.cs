using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;
using SportsHub.Web.Validators;
using Xunit;

namespace SportsHub.Web.Tests.Validators
{
    public class CreateLanguageModelValidatorTests
    {
        private readonly Mock<ILanguageService> _service;
        private readonly CreateLanguageModelValidator _validator;

        public CreateLanguageModelValidatorTests()
        {
            _service = new Mock<ILanguageService>();
            _validator = new CreateLanguageModelValidator(_service.Object);
        }

        [Fact]
        public async void CreateLanguageModel_WhenNameIsEmpty_ReturnsValidationResultWithError()
        {
            // Arrange
            var language = new CreateLanguageModel
            {
                Name = string.Empty
            };
            var expectedErrorMessage = Errors.LanguageNameCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(language);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateLanguageModel_WhenCodeIsEmpty_ReturnsValidationResultWithError()
        {
            // Arrange
            var language = new CreateLanguageModel
            {
                Code = string.Empty
            };
            var expectedErrorMessage = Errors.LanguageCodeCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(language);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateLanguageModel_WhenNameIsNotUnique_ReturnsValidationResultWithError()
        {
            // Arrange
            var language = new CreateLanguageModel
            {
                Name = "Name"
            };
            var expectedErrorMessage = Errors.LanguageNameIsNotUnique;

            _service.Setup(service => service.DoesLanguageAlreadyExistByNameAsync(language.Name))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.ValidateAsync(language);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateCategoryModel_WhenCodeIsNotUnique_ReturnsValidationResultWithError()
        {
            // Arrange
            var language = new CreateLanguageModel
            {
                Code = "Code"
            };
            var expectedErrorMessage = Errors.LanguageCodeIsNotUnique;

            _service.Setup(service => service.DoesLanguageAlreadyExistByCodeAsync(language.Code))
                .ReturnsAsync(true);

            // Act
            var result = await _validator.ValidateAsync(language);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void CreateLanguageModel_WhenModelIsValid_ReturnsSuccessValidationResult()
        {
            // Arrange
            var language = new CreateLanguageModel
            {
                Name = "Name",
                Code = "Code"
            };

            _service.Setup(service => service.DoesLanguageAlreadyExistByNameAsync(language.Name))
                .ReturnsAsync(false);
            _service.Setup(service => service.DoesLanguageAlreadyExistByCodeAsync(language.Code))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.ValidateAsync(language);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}
