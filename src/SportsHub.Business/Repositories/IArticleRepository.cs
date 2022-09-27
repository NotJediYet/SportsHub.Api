using SportsHub.Shared.Entities;

namespace SportsHub.Business.Repositories
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetArticlesAsync();

        Task<Article> GetArticleByIdAsync(Guid id);

        Task AddArticleAsync(Article article);

        Task<Article> DeleteArticleAsync(Guid id);

        Task<bool> DoesArticleAlreadyExistByHeadlineAsync(string headline);

        Task<bool> DoesArticleAlreadyExistByIdAsync(Guid id);

        IEnumerable<Article> GetArticlesFilteredByTeamId(Guid teamId, IEnumerable<Article> articles);

        IEnumerable<Article> GetArticlesFilteredByTeamsId(IEnumerable<Article> articles, ICollection<Team> teams);

        IEnumerable<Article> GetArticlesFilteredByStatus(string status, IEnumerable<Article> articles);

        Task<IEnumerable<Article>> GetSortedArticlesAsync();
    }
}