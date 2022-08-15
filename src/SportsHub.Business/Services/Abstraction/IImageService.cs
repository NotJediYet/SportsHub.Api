using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    public interface IImageService
    {
        Task<IEnumerable<Image>> GetImagesAsync();

        Task<Image> GetImageByIdAsync(Guid id);

        Task<bool> DoesImageAlreadyExistByBytesAsync(byte[] bytes);

        Task<bool> DoesImageAlreadyExistByIdAsync(Guid id);
    }
}