using Microsoft.AspNetCore.Http;
using SportsHub.Shared.Entities;

namespace SportsHub.Business.Repositories
{
    public interface IArticleImageRepository
    {
        Task<ArticleImage> GetImageByIdAsync(Guid id);

        Task<IEnumerable<ArticleImage>> GetImagesAsync();

        Task AddImageAsync(ArticleImage articleImage);

        Task<bool> DoesImageAlreadyExistByArticleIdAsync(Guid ArticleId);
    }
}