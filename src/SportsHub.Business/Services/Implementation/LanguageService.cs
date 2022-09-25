using SportsHub.Business.Repositories;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public class LanguageService : ILanguageService
    {
        private readonly ILanguageRepository _languageRepository;

        public LanguageService(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository ?? throw new ArgumentNullException(nameof(languageRepository));
        }

        public async Task<IEnumerable<Language>> GetLanguagesAsync()
        {
            return await _languageRepository.GetLanguagesAsync();
        }

        public async Task<Language> GetLanguageByIdAsync(Guid id)
        {
            return await _languageRepository.GetLanguageByIdAsync(id);
        }

        public async Task CreateLanguageAsync(CreateLanguageModel createLanguageModel)
        {
            var languageModel = new Language
            {
                Name = createLanguageModel.Name,
                Code = createLanguageModel.Code,
                IsDefault = createLanguageModel.IsDefault,
                IsHidden = createLanguageModel.IsHidden,
                IsAdded = createLanguageModel.IsAdded
            };

            await _languageRepository.AddLanguageAsync(languageModel);
        }

        public async Task EditLanguageAsync(EditLanguageModel editLanguageModel)
        {
            var languageModel = new Language
            {
                Id = editLanguageModel.Id,
                Name = editLanguageModel.Name,
                Code = editLanguageModel.Code,
                IsDefault = editLanguageModel.IsDefault,
                IsHidden = editLanguageModel.IsHidden,
                IsAdded = editLanguageModel.IsAdded
            };

            await _languageRepository.EditLanguageAsync(languageModel);
        }

        public async Task<Language> DeleteLanguageAsync(Guid id)
        {
            return await _languageRepository.DeleteLanguageAsync(id);
        }

        public async Task<Guid> GetLanguageIdByNameAsync(string languageName)
        {
            var language = await _languageRepository.GetLanguageByNameAsync(languageName);

            if (language == null)
            {
                return Guid.Empty;
            }
            else
            {
                return language.Id;
            }
        }

        public async Task<Guid> GetLanguageIdByCodeAsync(string languageCode)
        {
            var language = await _languageRepository.GetLanguageByCodeAsync(languageCode);

            if (language == null)
            {
                return Guid.Empty;
            }
            else
            {
                return language.Id;
            }
        }

        public async Task<bool> DoesLanguageAlreadyExistByNameAsync(string languageName)
        {
            return await _languageRepository.DoesLanguageAlreadyExistByNameAsync(languageName);
        }

        public async Task<bool> DoesLanguageAlreadyExistByCodeAsync(string languageCode)
        {
            return await _languageRepository.DoesLanguageAlreadyExistByCodeAsync(languageCode);
        }

        public async Task<bool> DoesLanguageAlreadyExistByIdAsync(Guid id)
        {
            return await _languageRepository.DoesLanguageAlreadyExistByIdAsync(id);
        }
    }
}
