using SportsHub.Shared.Entities;

namespace SportsHub.Business.Repositories
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetArticlesAsync();

        Task<Article> GetArticleByIdAsync(Guid id);

        Task AddArticleAsync(Article article);

        Task<bool> DoesArticleAlreadyExistByNameAsync(string headline);

        Task<bool> DoesArticleAlreadyExistByIdAsync(Guid id);

        IEnumerable<Article> GetArticlesFilteredByTeamId(Guid teamId, IEnumerable<Article> articles);

        IEnumerable<Article> GetArticlesFilteredByPublished(string isPublished, IEnumerable<Article> articles);

        Task<IEnumerable<Article>> GetSortedArticles();
    }
}



