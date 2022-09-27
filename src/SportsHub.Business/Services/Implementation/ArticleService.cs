using Microsoft.AspNetCore.Http;
using SportsHub.Business.Repositories;
using SportsHub.Extensions;
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
            var articles = await _articleRepository.GetArticlesAsync();
            var images = await _articleImageRepository.GetImagesAsync();

            foreach (var article in articles)
            {
                var image = images.FirstOrDefault(image => image.ArticleId == article.Id);

                article.Image = image;        
            }

           return articles;
        }

        public async Task<Article> GetArticleByIdAsync(Guid id)
        {
         var article = await _articleRepository.GetArticleByIdAsync(id);
         var image = await _articleImageRepository.GetImageByIdAsync(id);

         article.Image = image;
            
         return article;
        }

        public async Task CreateArticleAsync(CreateArticleModel сreateArticleModel)
        {
            var articleModel = new Article(
                сreateArticleModel.TeamId,
                сreateArticleModel.Location,
                сreateArticleModel.AltImage,
                сreateArticleModel.Headline,
                сreateArticleModel.Caption,
                сreateArticleModel.Content,
                сreateArticleModel.IsShowComments);
         
            await _articleRepository.AddArticleAsync(articleModel);

            if (сreateArticleModel.ArticleImage != null)
            {
                var fileBytes = сreateArticleModel.ArticleImage.ToByteArray();
                var fileExtension = Path.GetExtension(сreateArticleModel.ArticleImage.FileName);
                var newAricleImage = new ArticleImage(fileBytes, fileExtension, articleModel.Id);

                await _articleImageRepository.AddImageAsync(newAricleImage);
            }
        }

        public async Task<Article> DeleteArticleAsync(Guid id)
        {
            return await _articleRepository.DeleteArticleAsync(id);
        }

        public async Task<bool> DoesArticleAlreadyExistByHeadlineAsync(string headline)
        {
            return await _articleRepository.DoesArticleAlreadyExistByHeadlineAsync(headline);
        }

        public async Task<bool> DoesArticleAlreadyExistByIdAsync(Guid id)
        {
            return await _articleRepository.DoesArticleAlreadyExistByIdAsync(id);
        }

        public IEnumerable<Article> GetArticlesFilteredByTeamId(Guid teamId, IEnumerable<Article> articles)
        {
            return _articleRepository.GetArticlesFilteredByTeamId(teamId, articles);
        }

        public IEnumerable<Article> GetArticlesFilteredByStatus(string status, IEnumerable<Article> articles)
        {
            return _articleRepository.GetArticlesFilteredByStatus(status, articles);
        }

        public async Task<IEnumerable<Article>> GetSortedArticlesAsync()
        {
            return await _articleRepository.GetSortedArticlesAsync();
        }
    }
}