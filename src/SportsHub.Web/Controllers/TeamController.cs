using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Business.Services.Abstraction;
using SportsHub.Shared.Models;
using SportsHub.Web.Security;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _teamService.GetAllAsync());
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryAsync(Guid Id)
        {
            var Team = await _teamService.GetByIDAsync(Id);
            return Team != null ? Ok(Team) : NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Admin)]
        public async Task<IActionResult> CreateSubcategoryAsync(Team Team)
        {
            if (await _teamService.CheckIfSubcategoryIdNotExists(
                Team.SubcategoryId))
            {
                return BadRequest("Subcategory with that id doesn't exist!");
            }
            if ((Team.Id != Guid.Empty) &&
                (await _teamService.CheckIfNameNotUniqueAsync(Team.Name)))
            {
                return BadRequest("Team with that id already exists!");
            }
            if (await _teamService.CheckIfNameNotUniqueAsync(Team.Name))
            {
                return BadRequest("Team with that name already exists!");
            }
            await _teamService.CreateAsync(Team);
            return Ok();
        }
    }
}
