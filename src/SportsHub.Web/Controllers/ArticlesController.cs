﻿using Microsoft.AspNetCore.Authorization;
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
        private IValidator<CreateArticleModel> _createArticleModelValidator;


        public ArticlesController(
            IArticleService articleService,
            IValidator<CreateArticleModel> createArticleModelValidator)
        {
            _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
            _createArticleModelValidator = createArticleModelValidator ?? throw new ArgumentNullException(nameof(createArticleModelValidator));
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
        public async Task<IActionResult> CreateArticle([FromForm]CreateArticleModel сreateArticleModel)
        {
            var result = await _createArticleModelValidator.ValidateAsync(сreateArticleModel);
            if (!result.IsValid)
            {
                return BadRequest(result.ToString());
            }

            await _articleService.CreateArticleAsync(сreateArticleModel);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            var article = await _articleService.DeleteArticleAsync(id);
            
            return article != null
                ? Ok(article)
                : NotFound();
        }
    }
}