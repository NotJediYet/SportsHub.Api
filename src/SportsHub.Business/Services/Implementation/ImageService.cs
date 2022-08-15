using SportsHub.Business.Repositories;
using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
  public class ImageService : IImageService
    {
        private readonly IImageRepository _ImageRepository;

        public ImageService(IImageRepository ImageRepositor)
        {
            _ImageRepository = ImageRepositor ?? throw new ArgumentNullException(nameof(ImageRepositor));
        }

        public async Task<IEnumerable<Image>> GetImagesAsync()
        {
            return await _ImageRepository.GetImagesAsync();
        }

        public async Task<Image> GetImageByIdAsync(Guid id)
        {
            return await _ImageRepository.GetImageByIdAsync(id);
        }

        public async Task<bool> DoesImageAlreadyExistByBytesAsync(byte[] bytes)
        {

            return await _ImageRepository.DoesImageAlreadyExistByBytesAsync(bytes);
        }

        public async Task<bool> DoesImageAlreadyExistByIdAsync(Guid id)
        {
            return await _ImageRepository.DoesImageAlreadyExistByIdAsync(id);
        }
    }
}