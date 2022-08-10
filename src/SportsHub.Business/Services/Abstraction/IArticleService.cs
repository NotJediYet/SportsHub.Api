using SportsHub.Shared.Entities;

namespace SportsHub.Business.Services
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetArticlesAsync();

        Task<Article> GetArticleByIdAsync(Guid id);

        Task CreateArticleAsync(string articlePicture, Guid teamId, string location, string articleAltPicture, string articleHeadline, string articleCaption, string articleContext);

        Task<bool> DoesArticleAlreadyExistByNameAsync(string headline);

        Task<bool> DoesArticleAlredyExistByIdAsync(Guid id);

    }
}
