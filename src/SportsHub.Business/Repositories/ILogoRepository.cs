using SportsHub.Shared.Entities;

namespace SportsHub.Business.Repositories
{
    public interface ILogoRepository {
        Task<IEnumerable<Logo>> GetLogosAsync();

        Task<Logo> GetLogoByIdAsync(Guid id);

        Task AddLogoAsync(Logo logo);

        Task<bool> DoesLogoAlreadyExistByBytesAsync(byte[] bytes);

        Task<bool> DoesLogoAlreadyExistByIdAsync(Guid id); 
    }
}