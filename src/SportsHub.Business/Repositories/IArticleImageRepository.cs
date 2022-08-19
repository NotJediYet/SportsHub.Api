using Microsoft.AspNetCore.Http;
using SportsHub.Shared.Entities;

namespace SportsHub.Business.Repositories
{
    public interface IArticleImageRepository
    {
        Task<ArticleImage> GetImageByIdAsync(Guid id);

        Task AddImageAsync(IFormFile Image, Guid ArticleId);

        Task<bool> DoesImageAlreadyExistByArticleIdAsync(Guid ArticleId);
    }
}