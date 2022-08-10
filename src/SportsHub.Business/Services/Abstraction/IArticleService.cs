using Microsoft.AspNetCore.Http;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetArticlesAsync();

        Task<Article> GetArticleByIdAsync(Guid id);

        Task CreateArticleAsync(CreateArticleModel articleModel);

        Task<bool> DoesArticleAlreadyExistByHeadlineAsync(string headline);

        Task<bool> DoesArticleAlreadyExistByIdAsync(Guid id);

        IEnumerable<Article> GetArticlesFilteredByTeamId(Guid teamId, IEnumerable<Article> articles);

        IEnumerable<Article> GetArticlesFilteredByStatus(string status, IEnumerable<Article> articles);

        Task<IEnumerable<Article>> GetSortedArticlesAsync();
    }
}
