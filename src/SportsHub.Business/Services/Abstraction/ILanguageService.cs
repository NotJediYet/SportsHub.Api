using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public interface ILanguageService
    {
        Task<IEnumerable<Language>> GetLanguagesAsync();

        Task<Language> GetLanguageByIdAsync(Guid id);

        Task CreateLanguageAsync(CreateLanguageModel createLanguageModel);

        Task EditLanguageAsync(EditLanguageModel editLanguageModel);
        
        Task<Language> DeleteLanguageAsync(Guid Id);

        Task<Guid> GetLanguageIdByNameAsync(string languageName);

        Task<Guid> GetLanguageIdByCodeAsync(string languageCode);

        Task<bool> DoesLanguageAlreadyExistByNameAsync(string languageName);

        Task<bool> DoesLanguageAlreadyExistByCodeAsync(string languageCode);

        Task<bool> DoesLanguageAlreadyExistByIdAsync(Guid id);
    }
}
