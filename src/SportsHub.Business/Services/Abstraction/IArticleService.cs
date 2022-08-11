using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetArticlesAsync();

        Task<Article> GetArticleByIdAsync(Guid id);

        Task CreateArticleAsync(CreateArticleModel createArticleModel);

        Task<bool> DoesArticleAlreadyExistByNameAsync(string articleName);

        Task<bool> DoesArticleAlreadyExistByIdAsync(Guid id);
    }
}
