using SportsHub.Business.Repositories;
using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    internal class LogoService : ILogoService
    {
        private readonly ILogoRepository _logoRepository;

        public LogoService(ILogoRepository logoRepositor)
        {
            _logoRepository = logoRepositor ?? throw new ArgumentNullException(nameof(logoRepositor));
        }

        public async Task<IEnumerable<Logo>> GetLogosAsync()
        {
            return await _logoRepository.GetLogosAsync();
        }

        public async Task<Logo> GetLogoByIdAsync(Guid id)
        {
            var logo = await _logoRepository.GetLogoByIdAsync(id);

            return logo;
        }

        public async Task CreateLogoAsync(byte[] bytes, DateTime uploadDate, string fileExtension, Guid teamId)
        {
            await _logoRepository.AddLogoAsync(new Logo(bytes, uploadDate, fileExtension, teamId));

        }

        public async Task<bool> DoesLogoAlreadyExistByBytesAsync(byte[] bytes)
        {
            var result = await _logoRepository.DoesLogoAlreadyExistByBytesAsync(bytes);

            return result;
        }

        public async Task<bool> DoesLogoAlreadyExistByIdAsync(Guid id)
        {
            var result = await _logoRepository.DoesLogoAlreadyExistByIdAsync(id);

            return result;
        }
    }
}
