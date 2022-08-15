using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetArticlesAsync();

        Task<Article> GetArticleByIdAsync(Guid id);

        Task CreateArticleAsync(CreateArticleModel createArticleModel, CreateImageModel createImageModel);

        Task<bool> DoesArticleAlreadyExistByHeadlineAsync(string headline);

        Task<bool> DoesArticleAlreadyExistByIdAsync(Guid id);
    }
}