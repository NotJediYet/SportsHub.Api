using Moq;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Shared.Resources;
using SportsHub.Web.Validators;
using System;
using Xunit;
using System.IO;
using Microsoft.AspNetCore.Http;
using SportsHub.Shared.Entities;

namespace SportsHub.Web.Tests.Validators
{
    public class EditTeamModelValidatorTests
    {
        private readonly Mock<ISubcategoryService> _subcategoryService;
        private readonly Mock<ITeamService> _teamService;
        private readonly EditTeamModelValidator _validator;

        public EditTeamModelValidatorTests()
        {
            _subcategoryService = new Mock<ISubcategoryService>();
            _teamService = new Mock<ITeamService>();
            _validator = new EditTeamModelValidator(_subcategoryService.Object, _teamService.Object);
        }

        [Fact]
        public async void EditTeamModel_WhenTeamIdIsEmpty_ReturnsValidationResultWithError()
        {
            // Arrange
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            var team = new Team
            {
                Id = Guid.Empty,
                Name = "Name",
                Location = "Location",
                SubcategoryId = Guid.NewGuid(),
                TeamLogo = file
            };
            var expectedErrorMessage = Errors.TeamIdCanNotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditTeamModel_WhenTeamIdDoesNotExist_ReturnsValidationResultWithError()
        {
            // Arrange
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Location = "Location",
                SubcategoryId = Guid.NewGuid(),
                TeamLogo = file
            };
            var expectedErrorMessage = Errors.TeamIdDoesNotExist;

            _teamService.Setup(service => service.DoesTeamAlreadyExistByIdAsync(team.Id))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditTeamModel_WhenTeamNameIsEmpty_ReturnsValidationResultWithError()
        {
            // Arrange
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                Location = "Location",
                SubcategoryId = Guid.NewGuid(),
                TeamLogo = file
            };
            var expectedErrorMessage = Errors.TeamNameCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditTeamModel_WhenTeamNameAlreadyExist_ReturnsValidationResultWithError()
        {
            // Arrange
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Location = "Location",
                SubcategoryId = Guid.NewGuid(),
                TeamLogo = file
            };
            var expectedErrorMessage = Errors.TeamNameIsNotUnique;

            _teamService.Setup(service => service.DoesTeamAlreadyExistByNameAsync(team.Name))
                .ReturnsAsync(Guid.NewGuid());

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditTeamModel_WhenSubCategoryIdIsEmpty_ReturnsValidationResultWithError()
        {
            // Arrange
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Location = "Location",
                SubcategoryId = Guid.Empty,
                TeamLogo = file
            };
            var expectedErrorMessage = Errors.SubcategoryIdCannotBeEmpty;

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditTeamModel_WhenSubCategoryDoesNotExist_ReturnsValidationResultWithError()
        {
            // Arrange
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Location = "Location",
                SubcategoryId = Guid.NewGuid(),
                TeamLogo = file
            };
            var expectedErrorMessage = Errors.SubcategoryDoesNotExist;

            _subcategoryService.Setup(service => service.DoesSubcategoryAlreadyExistByIdAsync(team.SubcategoryId))
                .ReturnsAsync(false);

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditTeamModel_WhenTeamLogoHasWrongExtention_ReturnsValidationResultWithError()
        {
            // Arrange
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Location = "Location",
                SubcategoryId = Guid.NewGuid(),
                TeamLogo = file
            };
            var expectedErrorMessage = Errors.FileMustHaveAppropriateFormat;

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorMessage == expectedErrorMessage);
        }

        [Fact]
        public async void EditTeamModel_WhenModelIsValid_ReturnsSuccessValidationResult()
        {
            // Arrange
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var fileName = "test.png";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            //create FormFile
            IFormFile file = new FormFile(stream, 0, stream.Length, "id_from_form", fileName);

            var team = new Team
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Location = "Location",
                SubcategoryId = Guid.NewGuid(),
                TeamLogo = file
            };

            _teamService.Setup(service => service.DoesTeamAlreadyExistByIdAsync(team.Id))
            .ReturnsAsync(true);
            _teamService.Setup(service => service.DoesTeamAlreadyExistByNameAsync(team.Name))
            .ReturnsAsync(Guid.Empty);
            _subcategoryService.Setup(service => service.DoesSubcategoryAlreadyExistByIdAsync(team.SubcategoryId))
            .ReturnsAsync(true);

            // Act
            var result = await _validator.ValidateAsync(team);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }
    }
}
