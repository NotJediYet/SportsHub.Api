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
    public class FilterArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ITeamService _teamService;
        private readonly ISubcategoryService _subcategoryService;

        public FilterArticlesController(
            IArticleService articleService,
            ITeamService teamService,
            ISubcategoryService subcategoryService)
        {
            _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));
        }

        [HttpGet("{subcategoryName}/{teamName}/{isPublished}")]
        [Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<IActionResult> GetFilteredArticles(string subcategoryName, string teamName, string isPublished)
        {
            var articles = await _articleService.GetSortedArticles();

            if (teamName != "All Teams")
            {
                var idTeam = await _teamService.FindTeamIdByTeamName(teamName);

                articles = _articleService.GetArticlesFilteredByTeamId(idTeam, articles);
            }
            if (isPublished != "All")
            {
                articles = _articleService.GetArticlesFilteredByPublished(isPublished, articles);
            }
            if (subcategoryName != "All Subcategories")
            {
                var idSubcategory = await _subcategoryService.FindSubcategoryIdBySubcategoryName(subcategoryName);

                var idTeam = await _teamService.FindTeamIdBySubcategoryId(idSubcategory);

                articles = _articleService.GetArticlesFilteredByTeamId(idTeam, articles);
            }

            return articles != null
                ? Ok(articles)
                : NotFound();
        }
    }
}




