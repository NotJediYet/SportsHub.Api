using Microsoft.AspNetCore.Http;
using SportsHub.Business.Repositories;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleImageRepository _articleImageRepository;


        public ArticleService(IArticleRepository articleRepository, IArticleImageRepository articleImageRepository)
        {
            _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
            _articleImageRepository = articleImageRepository ?? throw new ArgumentNullException(nameof(articleImageRepository));
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync()
        {
            return await _articleRepository.GetArticlesAsync();
        }

        public async Task<Article> GetArticleByIdAsync(Guid id)
        {
            return  await _articleRepository.GetArticleByIdAsync(id);
        }

        public async Task CreateArticleAsync(Guid teamId, string location, string headline, string caption, string context, IFormFile articleImage)
        {
          Article newArticle = new Article(teamId, location,headline,caption,context);
            await _articleRepository.AddArticleAsync(newArticle);
            await _articleImageRepository.AddImageAsync(articleImage, newArticle.Id);
        }

        public async Task<bool> DoesArticleAlreadyExistByHeadlineAsync(string headline)
        {
            return await _articleRepository.DoesArticleAlreadyExistByHeadlineAsync(headline);
        }

        public async Task<bool> DoesArticleAlreadyExistByIdAsync(Guid id)
        {
            return await _articleRepository.DoesArticleAlreadyExistByIdAsync(id);
        }
    }
}