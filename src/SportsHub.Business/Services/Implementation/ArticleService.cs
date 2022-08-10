using SportsHub.Business.Repositories;
using SportsHub.Shared.Entities;


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

        public async Task CreateArticleAsync(string picture, Guid teamId, string location, string altPicture, string headline, string caption, string context)
        {
            await _articleRepository.AddArticleAsync(new Article(picture, teamId, location, altPicture, headline, caption, context));

        }

        public async Task<bool> DoesArticleAlreadyExistByNameAsync(string headline)
        {
            var result = await _articleRepository.DoesArticleAlreadyExistByNameAsync(headline);

            return result;
        }

        public async Task<bool> DoesArticleAlredyExistByIdAsync(Guid id)
        {
            var result = await _articleRepository.DoesArticleAlredyExistByIdAsync(id);

            return result;
        }
    }
}

