using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Security;
using FluentValidation;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IImageService _imageService;
        private IValidator<CreateArticleModel> _createArticleModelValidator;
        private IValidator<CreateImageModel> _createImageModelValidator;

        public ArticlesController(
            IArticleService articleService,
            IImageService imageService,
            IValidator<CreateArticleModel> createArticleModelValidator,
            IValidator<CreateImageModel> createImageModelValidator)
        {
            _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
            _createArticleModelValidator = createArticleModelValidator ?? throw new ArgumentNullException(nameof(createArticleModelValidator));
            _imageService = imageService ?? throw new ArgumentNullException(nameof(imageService));
            _createImageModelValidator = createImageModelValidator ?? throw new ArgumentNullException(nameof(createImageModelValidator));

        }
        [HttpGet]
        [Authorize(Policies.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetImages()
        {
            return Ok(await _imageService.GetImagesAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Policies.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetImages(Guid id)
        {
            var Image = await _imageService.GetImageByIdAsync(id);
            return Image != null
                ? Ok(Image)
                : NotFound();
        }

        [HttpGet]
        [Authorize(Policies.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetArticles()
        {
            var articles = await _articleService.GetArticlesAsync();
            return Ok(articles);
        }


        [HttpGet("{id}")]
        [Authorize(Policies.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetArticle(Guid id)
        {
            var article = await _articleService.GetArticleByIdAsync(id);

            return article != null
                ? Ok(article)
                : NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]


        public async Task<IActionResult> CreateArticle(CreateArticleModel сreateArticleModel, CreateImageModel createImageModel)
        {
            var result = await _createArticleModelValidator.ValidateAsync(сreateArticleModel);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));
            }
           result = await _createImageModelValidator.ValidateAsync(createImageModel);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));
            }

            await _articleService.CreateArticleAsync(сreateArticleModel, createImageModel);

            return Ok();
        }
    }
}