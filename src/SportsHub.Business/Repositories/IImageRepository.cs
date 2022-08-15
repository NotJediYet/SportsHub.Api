using SportsHub.Shared.Entities;

namespace SportsHub.Business.Repositories
{
    public interface IImageRepository
    {
        Task<IEnumerable<Image>> GetImagesAsync();

        Task<Image> GetImageByIdAsync(Guid id);

        Task AddImageAsync(Image image);

        Task<bool> DoesImageAlreadyExistByBytesAsync(byte[] bytes);

        Task<bool> DoesImageAlreadyExistByIdAsync(Guid id);
    }
}