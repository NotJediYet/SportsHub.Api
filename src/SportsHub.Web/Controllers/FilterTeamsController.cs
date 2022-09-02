using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Business.Services;
using SportsHub.Security;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterTeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly ISubcategoryService _subcategoryService;
        private readonly ICategoryService _categoryService;

        public FilterTeamsController(
            ITeamService teamService,
            ISubcategoryService subcategoryService,
            ICategoryService categoryService)
        {
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpGet("{location}/{categoryName}/{subcategoryName}")]
        [Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<IActionResult> GetFilteredTeams(string location, string categoryName, string subcategoryName)
        {
            var teams = await _teamService.GetSortedTeamAsync();

            if (categoryName != "All")
            {
                var idCategory = await _categoryService.FindCategoryIdByCategoryNameAsync(categoryName);
                var subcategoryIds = await _subcategoryService.FindSubcategoryIdByCategoryIdAsync(idCategory);

                teams = _teamService.GetTeamsFilteredBySubcategoryIds(subcategoryIds, teams);
            }

            if (location != "All")
            {
                teams = _teamService.GetTeamsFilteredByLocation(location, teams);
            }

            if (subcategoryName != "All")
            {
                var idSubcategory = await _subcategoryService.FindSubcategoryIdBySubcategoryNameAsync(subcategoryName);

                teams = _teamService.GetTeamsFilteredBySubcategoryId(idSubcategory, teams);
            }

            return teams != null
                ? Ok(teams)
                : NotFound();
        }
    }
}

