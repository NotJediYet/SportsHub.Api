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
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _teamService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetCategoryAsync(Guid id)
        {
            var team = await _teamService.GetByIdAsync(id);
            return team != null ? Ok(team) : NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Admin)]
        public async Task<IActionResult> CreateSubcategoryAsync(
            CreateTeamModel newTeam)
        {
            if (await _teamService.CheckIfSubcategoryIdNotExists(
                    newTeam.SubcategoryId))
            {
                return BadRequest("Subcategory with that id doesn't exist!");
            }
            if (await _teamService.CheckIfNameNotUniqueAsync(newTeam.Name))
            {
                return BadRequest("Team with that name already exists!");
            }

            await _teamService.CreateAsync(
                newTeam.Name, newTeam.SubcategoryId);

            return Ok();
        }
    }
}
