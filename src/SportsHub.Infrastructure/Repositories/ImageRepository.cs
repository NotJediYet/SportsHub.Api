using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Repositories;

namespace SportsHub.Infrastructure.Repositories
{
    internal class ImageRepository : IImageRepository
    {
        readonly protected SportsHubDbContext _context;

        public ImageRepository(SportsHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Image>> GetImagesAsync()
        {
            return await _context.Set<Image>().ToListAsync();
        }

        public async Task<Image> GetImageByIdAsync(Guid id)
        {
            return await _context.Set<Image>().FindAsync(id);
        }

        public async Task AddImageAsync(Image image)
        {
            await _context.Set<Image>().AddAsync(image);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesImageAlreadyExistByBytesAsync(byte[] bytes)
        {
            var logos = await _context.Set<Image>().ToListAsync();

            return logos.Any(image => image.Bytes == bytes);
        }

        public async Task<bool> DoesImageAlreadyExistByIdAsync(Guid id)
        {
            var images = await _context.Set<Image>().ToListAsync();

            return images.Any(image => image.Id == id);
        }
    }
}