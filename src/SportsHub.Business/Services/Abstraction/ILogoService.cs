using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    public interface ILogoService
    {
        Task<IEnumerable<Logo>> GetLogosAsync();

        Task<Logo> GetLogoByIdAsync(Guid id);

        Task CreateLogoAsync(byte[] bytes, DateTime uploadDate, string fileExtension, Guid teamId);

        Task<bool> DoesLogoAlreadyExistByBytesAsync(byte[] bytes);

        Task<bool> DoesLogoAlreadyExistByIdAsync(Guid id);
    }
}
