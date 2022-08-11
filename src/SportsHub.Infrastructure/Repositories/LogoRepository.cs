using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;
using Microsoft.EntityFrameworkCore;
using SportsHub.Business.Repositories;

namespace SportsHub.Infrastructure.Repositories
{
    internal class LogoRepository : ILogoRepository
    {
        readonly protected SportsHubDbContext _context;

        public LogoRepository(SportsHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Logo>> GetLogosAsync()
        {
            return await _context.Set<Logo>().ToListAsync();
        }

        public async Task<Logo> GetLogoByIdAsync(Guid id)
        {
            return await _context.Set<Logo>().FindAsync(id);
        }

        public async Task AddLogoAsync(Logo logo)
        {
            await _context.Set<Logo>().AddAsync(logo);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesLogoAlreadyExistByBytesAsync(byte[] bytes)
        {
            var logos = await _context.Set<Logo>().ToListAsync();

            return logos.Any(logo => logo.Bytes == bytes);
        }

        public async Task<bool> DoesLogoAlreadyExistByIdAsync(Guid id)
        {
            var logos = await _context.Set<Logo>().ToListAsync();

            return logos.Any(logo => logo.Id == id);
        }
    }
}
