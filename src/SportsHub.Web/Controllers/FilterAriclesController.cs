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
    public class FilterAriclesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ITeamService _teamService;

        public FilterAriclesController(
            IArticleService articleService,
            ITeamService teamService)
        {
            _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
        }

        [HttpGet("{teamName}")]
        [Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult GetFileredArticlesByTeam(string teamName)
        {
            var idTeam = _teamService.FindTeamIdByTeamName(teamName);

            var articles = _articleService.GetArticlesFilteredByTeamId(idTeam);

            return articles != null
                ? Ok(articles)
                : NotFound();
        }
    }
}

