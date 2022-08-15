using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Business.Services;
using SportsHub.Security;
using SportsHub.Shared.Models;
using Microsoft.AspNetCore.Http;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private IValidator<CreateTeamModel> _createTeamModelValidator;

        public TeamsController(
            ITeamService teamService,
            IValidator<CreateTeamModel> createTeamModelValidator)
        {
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
            _createTeamModelValidator = createTeamModelValidator ?? throw new ArgumentNullException(nameof(createTeamModelValidator));
        }

        [HttpGet]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetTeams()
        {
            return Ok(await _teamService.GetTeamsAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetTeam(Guid id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            return team != null 
                ? Ok(team) 
                : NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateTeam([FromForm] CreateTeamModel сreateTeamModel)
        {
            var result = await _createTeamModelValidator.ValidateAsync(сreateTeamModel);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));
            }

            await _teamService.CreateTeamAsync(сreateTeamModel.Name, сreateTeamModel.SubcategoryId,
                                               сreateTeamModel.Location, сreateTeamModel.TeamLogo);

            return Ok();
        }
    }
}
