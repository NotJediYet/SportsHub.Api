using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Repositories;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;

namespace SportsHub.Infrastructure.Repositories
{
    internal class LanguageRepository : ILanguageRepository
    {
        readonly protected SportsHubDbContext _context;

        public LanguageRepository(SportsHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Language>> GetLanguagesAsync()
        {
            return await _context.Languages.ToListAsync();
        }

        public async Task<Language> GetLanguageByIdAsync(Guid id)
        {
            return await _context.Languages.FindAsync(id);
        }

        public async Task<Language> GetLanguageByNameAsync(string languageName)
        {
            return await _context.Languages.FirstOrDefaultAsync(language => language.Name == languageName);
        }

        public async Task<Language> GetLanguageByCodeAsync(string languageCode)
        {
            return await _context.Languages.FirstOrDefaultAsync(language => language.Code == languageCode);
        }

        public async Task AddLanguageAsync(Language language)
        {
            await _context.Languages.AddAsync(language);

            await _context.SaveChangesAsync();
        }

        public async Task EditLanguageAsync(Language language)
        {
            var oldLanguage = await _context.Languages.FirstOrDefaultAsync(oldLanguage => oldLanguage.Id == language.Id);

            oldLanguage.Name = language.Name;
            oldLanguage.Code = language.Code;
            oldLanguage.IsDefault = language.IsDefault;
            oldLanguage.IsHidden = language.IsHidden;
            oldLanguage.IsAdded = language.IsAdded;

            await _context.SaveChangesAsync();
        }

        public async Task<Language> DeleteLanguageAsync(Guid id)
        {
            var language = _context.Languages.Find(id);
            if (language != null)
            {
                _context.Languages.Remove(language);
                await _context.SaveChangesAsync();
            }

            return language;
        }

        public async Task<bool> DoesLanguageAlreadyExistByNameAsync(string languageName)
        {
            var languages = await _context.Languages.AnyAsync(language => language.Name == languageName);

            return languages;
        }

        public async Task<bool> DoesLanguageAlreadyExistByCodeAsync(string languageCode)
        {
            var languages = await _context.Languages.AnyAsync(language => language.Code == languageCode);

            return languages;
        }

        public async Task<bool> DoesLanguageAlreadyExistByIdAsync(Guid id)
        {
            var languages = await _context.Languages.AnyAsync(language => language.Id == id);

            return languages;
        }
    }
}
