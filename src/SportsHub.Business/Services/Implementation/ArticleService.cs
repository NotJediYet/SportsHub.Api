using SportsHub.Business.Repositories;
using SportsHub.Shared.Entities;
using SportsHub.Shared.Models;

namespace SportsHub.Business.Services
{
    public class ArticleService : IArticleService
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
            return  await _articleRepository.GetArticleByIdAsync(id);
        }

        public async Task CreateArticleAsync(CreateArticleModel createArticleModel, CreateImageModel createImageModel)
        {
            var article = new Article(
               createArticleModel.TeamId,
               createArticleModel.Location,
               createArticleModel.Headline,
               createArticleModel.Caption,
               createArticleModel.Context);

           var image = new Image(
               createImageModel.Bytes,
               createImageModel.ImageName,
               createImageModel.FileExtension,
               createImageModel.ImageSize,
               createImageModel.ArticleId);

            article.Image = image;

            await _articleRepository.AddArticleAsync(article);
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