using FluentValidation;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;

namespace SportsHub.Web.Validators
{
    internal class CreateImageModelValidator : AbstractValidator<CreateImageModel>
    {
        private readonly IImageService _imageService;
        private readonly IArticleService _articleService;

        public CreateImageModelValidator(
            IImageService imageService,
            IArticleService articleService)
        {

            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));

            RuleFor(image => image.Bytes)
                .NotEmpty().WithMessage("Image data can not be empty!")
                .MustAsync((bytes, cancellation) => DoesImageDataIsUniqueAsync(bytes)).WithMessage("This image already exists!");

            RuleFor(image => image.ArticleId)
                .NotEmpty().WithMessage("Team id can not be empty!")
                .MustAsync((id, cancellation) => _articleService.DoesArticleAlreadyExistByIdAsync(id))
                .WithMessage("Article with that id does not exist!");
        }

        private async Task<bool> DoesImageDataIsUniqueAsync(byte[] bytes)
        {
            var result = await _imageService.DoesImageAlreadyExistByBytesAsync(bytes);

            return !result;
        }
    }
}