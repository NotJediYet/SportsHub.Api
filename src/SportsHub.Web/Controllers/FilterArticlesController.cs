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
        private readonly ICategoryService _categoryService;

        public FilterArticlesController(
            IArticleService articleService,
            ITeamService teamService,
            ISubcategoryService subcategoryService,
            ICategoryService categoryService)
        {
            _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpGet("{categoryName}/{subcategoryName}/{teamName}/{status}")]
        [Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<IActionResult> GetFilteredArticles(string categoryName, string subcategoryName, string teamName, string status)
        {
            var articles = await _articleService.GetArticlesAsync();
          
            var categoryId = await _categoryService.FindCategoryIdByCategoryNameAsync(categoryName);

            if (subcategoryName != "All Subcategories")
            {
                var subcategoryId = await _subcategoryService.FindSubcategoryIdBySubcategoryNameAsync(subcategoryName);

                var subcategory = await _subcategoryService.GetSubcategoryByIdAsync(subcategoryId);

                if (subcategory.CategoryId == categoryId)
                {
                    if (teamName != "All Teams")
                    {
                        var teamId = await _teamService.GetTeamIdByNameAsync(teamName);

                        var team = await _teamService.GetTeamByIdAsync(teamId);

                        if (team.SubcategoryId == subcategoryId)
                        {
                            articles = _articleService.GetArticlesFilteredByTeamId(teamId, articles);
                        }
                        else
                        {
                            articles = null;
                        }
                    }
                    else
                    {
                        var teams = await _teamService.GetTeamsAsync();

                        teams = _teamService.GetTeamsFilteredBySubcategoryId(subcategoryId, teams.ToList());

                        articles = _articleService.GetArticlesFilteredByTeamsId(articles, teams.ToList());
                    }
                }
                else { articles = null; }
            }
            else
            {
                var subcategories = await _subcategoryService.GetSubcategoriesByCategoryIdAsync(categoryId);
                var teams = await _teamService.GetTeamsAsync();
                teams = _teamService.GetTeamsFilteredBySubcategoryIds(subcategories, teams.ToList());

                if (teamName != "All Teams")
                {
                    var teamId = await _teamService.GetTeamIdByNameAsync(teamName);

                    var foundTeam = teams.ToList().Find(teamm => teamm.Id == teamId);

                    if (foundTeam != null)
                    {
                        articles = _articleService.GetArticlesFilteredByTeamId(teamId, articles);
                    }
                    else { articles = null; }
                }
                else
                {
                    articles = _articleService.GetArticlesFilteredByTeamsId(articles, teams.ToList());
                }
            }
            if (status != "All")
            {
                articles = _articleService.GetArticlesFilteredByStatus(status, articles);
            }
           
            return articles != null
                ? Ok(articles)
                : NotFound();
        }
    }
}

