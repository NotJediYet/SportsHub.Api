using SportsHub.Shared.Entities;

namespace SportsHub.Business.Repositories
{
    public interface ILanguageRepository
    {
        Task<IEnumerable<Language>> GetLanguagesAsync();

        Task<Language> GetLanguageByIdAsync(Guid id);

        Task<Language> GetLanguageByNameAsync(string languageName);

        Task<Language> GetLanguageByCodeAsync(string languageCode);

        Task AddLanguageAsync(Language language);

        Task EditLanguageAsync(Language language);

        Task<Language> DeleteLanguageAsync(Guid id);

        Task<bool> DoesLanguageAlreadyExistByNameAsync(string languageName);

        Task<bool> DoesLanguageAlreadyExistByCodeAsync(string languageCode);

        Task<bool> DoesLanguageAlreadyExistByIdAsync(Guid id);
    }
}
