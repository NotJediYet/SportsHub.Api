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

        [HttpGet("{subcategoryName}/{teamName}/{status}")]
        [AllowAnonymous]
        //[Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<IActionResult> GetFilteredArticles(string subcategoryName, string teamName, string status)
        {
            var articles = await _articleService.GetSortedArticlesAsync();

            if (teamName != "All Teams")
            {
                var idTeam = await _teamService.FindTeamIdByTeamNameAsync(teamName);

                articles = _articleService.GetArticlesFilteredByTeamId(idTeam, articles);
            }
            if (status != "All")
            {
                articles = _articleService.GetArticlesFilteredByStatus(status, articles);
            }
            if (subcategoryName != "All Subcategories")
            {
                var idSubcategory = await _subcategoryService.FindSubcategoryIdBySubcategoryNameAsync(subcategoryName);

                var idTeam = await _teamService.FindTeamIdBySubcategoryIdAsync(idSubcategory);

                articles = _articleService.GetArticlesFilteredByTeamId(idTeam, articles);
            }

            return articles != null
                ? Ok(articles)
                : NotFound();
        }
    }
}

