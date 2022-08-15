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
            return await _context.Images.ToListAsync();
        }

        public async Task<Image> GetImageByIdAsync(Guid id)
        {
            return await _context.Images.FindAsync(id);
        }

        public async Task AddImageAsync(Image image)
        {
            await _context.Images.AddAsync(image);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesImageAlreadyExistByBytesAsync(byte[] bytes)
        {
            return await _context.Images.AnyAsync((image => image.Bytes == bytes));
        }

        public async Task<bool> DoesImageAlreadyExistByIdAsync(Guid id)
        {
            return await _context.Images.AnyAsync(images => images.Id == id);
        }
    }
}