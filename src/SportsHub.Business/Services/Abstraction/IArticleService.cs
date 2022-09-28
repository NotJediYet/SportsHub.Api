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

        Task<Article> DeleteArticleAsync(Guid Id);

        Task<bool> DoesArticleAlreadyExistByHeadlineAsync(string headline);

        Task<bool> DoesArticleAlreadyExistByIdAsync(Guid id);

        IEnumerable<Article> GetArticlesFilteredByTeamId(Guid teamId, IEnumerable<Article> articles);
       
        IEnumerable<Article> GetArticlesFilteredByTeamsId(IEnumerable<Article> articles, ICollection<Team> teams);

        IEnumerable<Article> GetArticlesFilteredByStatus(string status, IEnumerable<Article> articles);
    }
}
