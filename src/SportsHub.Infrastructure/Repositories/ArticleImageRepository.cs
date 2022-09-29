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

        public async Task<IEnumerable<ArticleImage>> GetImagesAsync()
        {
            return await _context.Images.ToListAsync();
        }

        public async Task<ArticleImage> GetImageByIdAsync(Guid id)
        {
            return await _context.Images.FindAsync(id);
        }

        public async Task AddImageAsync(ArticleImage articleImageFile)
        {
            await _context.Set<ArticleImage>().AddAsync(articleImageFile);

            await _context.SaveChangesAsync();
        }
        public async Task<bool> DoesImageAlreadyExistByArticleIdAsync(Guid articleId)
        {
            return await _context.Images.AnyAsync(image => image.ArticleId == articleId);
        }
    }
}