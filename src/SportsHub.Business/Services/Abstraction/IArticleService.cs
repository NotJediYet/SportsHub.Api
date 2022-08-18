using Microsoft.AspNetCore.Http;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetArticlesAsync();

        Task<Article> GetArticleByIdAsync(Guid id);

        Task CreateArticleAsync(Article article, IFormFile Image);

        Task<bool> DoesArticleAlreadyExistByHeadlineAsync(string headline);

        Task<bool> DoesArticleAlreadyExistByIdAsync(Guid id);

        IEnumerable<Article> GetArticlesFilteredByTeamId(Guid teamId, IEnumerable<Article> articles);

        IEnumerable<Article> GetArticlesFilteredByStatus(string status, IEnumerable<Article> articles);

        Task<IEnumerable<Article>> GetSortedArticlesAsync();
    }
}