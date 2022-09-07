using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Repositories;
using Microsoft.AspNetCore.Http;

namespace SportsHub.Infrastructure.Repositories
{
    internal class ArticleImageRepository : IArticleImageRepository
    {
        readonly protected SportsHubDbContext _context;

        public ArticleImageRepository(SportsHubDbContext context)
        {
            _context = context;
        }

        public async Task<ArticleImage> GetImageByIdAsync(Guid id)
        {
            return await _context.Images.FindAsync(id);
        }

        public async Task AddImageAsync(IFormFile articleImageFile, Guid articleId)
        {
            var memoryStream = new MemoryStream();


            await articleImageFile.CopyToAsync(memoryStream);
            
            var bytes = memoryStream.ToArray();
            var imageExtension = Path.GetExtension(articleImageFile.FileName);

            ArticleImage newArticleImage = new ArticleImage(bytes, imageExtension, articleId);

            await articleImageFile.CopyToAsync(memoryStream);
            
            await _context.Images.AddAsync(newArticleImage);
            
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesImageAlreadyExistByArticleIdAsync(Guid articleId)
        {
            return await _context.Images.AnyAsync(image => image.ArticleId == articleId);
        }
    }
}