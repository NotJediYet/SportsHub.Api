using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;
using SportsHub.Web.Validators;
using System;
using Xunit;

namespace SportsHub.Web.Tests.Validators
{
    public class EditLanguageModelValidatorTests
    {
        private readonly Mock<ILanguageService> _service;
        private readonly EditLanguageModelValidator _validator;

        public EditLanguageModelValidatorTests()
        {
            _service = new Mock<ILanguageService>();
            _validator = new EditLanguageModelValidator(_service.Object);
        }

        [Fact]
        public async void EditLanguageModel_WhenLanguageIdIsEmpty_ReturnsValidationResultWithError()
        {
            var language = new EditLanguageModel
            {
                Id = Guid.Empty,
                Name = "Name",
                Code = "Code",
                IsDefault = false,
                IsHidden = true,
                IsAdded = false,
            };

            var expectedErrorMessage = Errors.LanguageIdCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(language);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditLanguageModel_WhenLanguageIdDoesNotExist_ReturnsValidationResultWithError()
        {
            var language = new EditLanguageModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Code = "Code",
                IsDefault = false,
                IsHidden = true,
                IsAdded = false,
            };

            var expectedErrorMessage = Errors.LanguageDoesNotExist;

            _service.Setup(service => service.DoesLanguageAlreadyExistByIdAsync(language.Id))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.ValidateAsync(language);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditLanguageModel_WhenLanguageNameIsEmpty_ReturnsValidationResultWithError()
        {
            var language = new EditLanguageModel
            {
                Id = Guid.NewGuid(),
                Name = "",
                Code = "Code",
                IsDefault = false,
                IsHidden = true,
                IsAdded = false,
            };

            var expectedErrorMessage = Errors.LanguageNameCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(language);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditLanguageModel_WhenLanguageNameAlreadyExist_ReturnsValidationResultWithError()
        {
            var language = new EditLanguageModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Code = "Code",
                IsDefault = false,
                IsHidden = true,
                IsAdded = false,
            };

            var expectedErrorMessage = Errors.LanguageNameIsNotUnique;

            _service.Setup(service => service.GetLanguageIdByNameAsync(language.Name))
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _validator.ValidateAsync(language);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditLanguageModel_WhenLanguageCodeIsEmpty_ReturnsValidationResultWithError()
        {
            var language = new EditLanguageModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Code = "",
                IsDefault = false,
                IsHidden = true,
                IsAdded = false,
            };

            var expectedErrorMessage = Errors.LanguageCodeCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(language);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditLanguageModel_WhenLanguageCodeAlreadyExist_ReturnsValidationResultWithError()
        {
            var language = new EditLanguageModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Code = "Code",
                IsDefault = false,
                IsHidden = true,
                IsAdded = false,
            };

            var expectedErrorMessage = Errors.LanguageCodeIsNotUnique;

            _service.Setup(service => service.GetLanguageIdByCodeAsync(language.Code))
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _validator.ValidateAsync(language);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }


        [Fact]
        public async void EditLanguageModel_WhenLanguageIsValid_ReturnsSuccessValidationResult()
        {
            var language = new EditLanguageModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Code = "Code",
                IsDefault = false,
                IsHidden = true,
                IsAdded = false,
            };

            _service.Setup(service => service.DoesLanguageAlreadyExistByIdAsync(language.Id))
                .ReturnsAsync(true);
            _service.Setup(service => service.GetLanguageIdByNameAsync(language.Name))
                .ReturnsAsync(Guid.Empty);
            _service.Setup(service => service.GetLanguageIdByCodeAsync(language.Code))
                .ReturnsAsync(Guid.Empty);

            // Act
            var result = await _validator.ValidateAsync(language);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}

