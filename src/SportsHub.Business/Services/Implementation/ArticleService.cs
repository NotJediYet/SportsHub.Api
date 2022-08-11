using SportsHub.Business.Repositories;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    internal class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;

        public ArticleService(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository ?? throw new ArgumentNullException(nameof(articleRepository));
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync()
        {
            return await _articleRepository.GetArticlesAsync();
        }

        public async Task<Article> GetArticleByIdAsync(Guid id)
        {
            var article = await _articleRepository.GetArticleByIdAsync(id);

            return article;
        }

        public async Task CreateArticleAsync(CreateArticleModel createArticleModel)
        {
            Article article = new Article
            (
               createArticleModel.Picture,
               createArticleModel.TeamId,
               createArticleModel.Location,
               createArticleModel.AltPicture,
               createArticleModel.Headline,
               createArticleModel.Caption,
               createArticleModel.Context
            );

            await _articleRepository.AddArticleAsync(article);
        }

        public async Task<bool> DoesArticleAlreadyExistByNameAsync(string articleName)
        {
            var result = await _articleRepository.DoesArticleAlreadyExistByNameAsync(articleName);

            return result;
        }

        public async Task<bool> DoesArticleAlreadyExistByIdAsync(Guid id)
        {
            var result = await _articleRepository.DoesArticleAlreadyExistByIdAsync(id);

            return result;
        }
    }
}

