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
    }
}



