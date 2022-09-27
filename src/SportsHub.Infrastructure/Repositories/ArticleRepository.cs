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

        public async Task<Article> DeleteArticleAsync(Guid id)
        {
            var article = _context.Articles.Find(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();
            }

            return article;
        }

        public async Task<bool> DoesArticleAlreadyExistByHeadlineAsync(string headline)
        {
            return await _context.Articles.AnyAsync(article => article.Headline == headline);
        }

        public async Task<bool> DoesArticleAlreadyExistByIdAsync(Guid id)
        {
            var articles = await _context.Articles.AnyAsync(article => article.Id == id);

            return articles;
        }

        public IEnumerable<Article> GetArticlesFilteredByTeamId(Guid teamId, IEnumerable<Article> articles)
        {
            articles = articles.Where(articles => articles.TeamId == teamId).ToList();

            return articles;
        }

        public IEnumerable<Article> GetArticlesFilteredByTeamsId(IEnumerable<Article> articles, ICollection<Team> teams)
        {
            var teamsId = teams.Select(team => team.Id);
            var newArticles = articles;
            articles = newArticles.Where(article => article.TeamId == teamsId.ToList()[0]).ToList();

            for (int i = 1; i < teamsId.ToList().Count; i++)
            {
                articles = articles.Concat(newArticles.Where(article => article.TeamId == teamsId.ToList()[i]).ToList()).ToList();
            }

            return articles;
        }

        public IEnumerable<Article> GetArticlesFilteredByStatus(string status, IEnumerable<Article> articles)
        {
            if (status == "Published")
            {
                articles = articles.Where(articles => articles.IsPublished == true).ToList();
            }
            else
        {
                articles = articles.Where(articles => articles.IsPublished == false).ToList();
            }

            return articles;
        }

        public async Task<IEnumerable<Article>> GetSortedArticlesAsync()
        {
            var articles = await _context.Articles
                .OrderBy(articles => articles.Headline)
                .ToListAsync();
       
            return articles;
        }
    }
}