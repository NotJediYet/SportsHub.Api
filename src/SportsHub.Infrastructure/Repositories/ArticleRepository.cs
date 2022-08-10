using SportsHub.Business.Repositories;
using SportsHub.Infrastructure.DBContext;
using SportsHub.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace SportsHub.Infrastructure.Repositories
{
    internal class ArticleRepository : IArticleRepository
    {
        readonly protected SportsHubDbContext _context;

        public ArticleRepository(SportsHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync()
        {
            return await _context.Set<Article>().ToListAsync();
        }

        public async Task<Article> GetArticleByIdAsync(Guid id)
        {
            return await _context.Set<Article>().FindAsync(id);
        }

        public async Task AddArticleAsync(Article article)
        {
            await _context.Set<Article>().AddAsync(article);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesArticleAlreadyExistByNameAsync(string headline)
        {
            var articles = await _context.Set<Article>().ToListAsync();

            return articles.Any(article => article.Headline == headline);
        }

        public async Task<bool> DoesArticleAlredyExistByIdAsync(Guid id)
        {
            var articles = await _context.Set<Article>().ToListAsync();

            return articles.Any(article => article.Id == id);
        }

       
    }
}

