using Microsoft.AspNetCore.Http;
using SportsHub.Shared.Entities;

namespace SportsHub.Business.Repositories
{
    public interface IArticleImageRepository
    {
        Task<IEnumerable<ArticleImage>> GetImagesAsync();

        Task<ArticleImage> GetImageByIdAsync(Guid id);

        Task AddImageAsync(IFormFile articleImageFile, Guid Article);

        Task<bool> DoesImageAlreadyExistByArticleIdAsync(Guid ArticleId);

    }
}