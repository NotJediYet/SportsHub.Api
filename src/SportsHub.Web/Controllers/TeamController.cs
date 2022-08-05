using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Business.Services.Abstraction;
using SportsHub.Shared.Models;
using SportsHub.Web.Security;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly ISubcategoryService _subcategoryService;

        public TeamsController(ITeamService teamService,
            ISubcategoryService subcategoryService)
        {
            _teamService = teamService;
            _subcategoryService = subcategoryService;
        }

        [HttpGet]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetAllTeams()
        {
            return Ok(await _teamService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetTeam(Guid id)
        {
            var team = await _teamService.GetByIdAsync(id);
            return team != null ? Ok(team) : NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Admin)]
        public async Task<IActionResult> CreateTeam(
            CreateTeamModel сreateTeamModel)
        {
            var doesSubcategoryExist = await _subcategoryService
                .DoesSubcategoryAlredyExistByIdAsync(сreateTeamModel.SubcategoryId);
            if (!doesSubcategoryExist)
            {
                return BadRequest("Subcategory with that id doesn't exist!");
            }

            var doesTeamExist = await _teamService
                .DoesTeamAlreadyExistByNameAsync(сreateTeamModel.Name);
            if (doesTeamExist)
            {
                return BadRequest("Team with that name already exists!");
            }

            await _teamService.CreateAsync(
                сreateTeamModel.Name, сreateTeamModel.SubcategoryId);

            return Ok();
        }
    }
}
