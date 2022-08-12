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
            return await _context.Articles.ToListAsync();
        }

        public async Task<Article> GetArticleByIdAsync(Guid id)
        {
            return await _context.Articles.FindAsync(id);
        }

        public async Task AddArticleAsync(Article article)
        {
            await _context.Articles.AddAsync(article);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> DoesArticleAlreadyExistByNameAsync(string headline)
        {
            var articles = await _context.Articles.AnyAsync(article => article.Headline == headline);

            return articles;
        }

        public async Task<bool> DoesArticleAlreadyExistByIdAsync(Guid id)
        {
            var articles = await _context.Articles.AnyAsync(article => article.Id == id);

            return articles;
        }

        public IEnumerable<Article> GetArticlesFilteredByTeamId(Guid teamId)
        {
           IQueryable<Article> articles = _context.Articles;

           articles = (from t in articles
                       where t.TeamId == teamId
                       select t);
        
           return  articles;
       }
     }
}






