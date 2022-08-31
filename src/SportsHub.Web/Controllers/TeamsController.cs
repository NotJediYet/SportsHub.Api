using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Business.Services;
using SportsHub.Security;
using SportsHub.Shared.Models;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private IValidator<CreateTeamModel> _createTeamModelValidator;
        private IValidator<EditTeamModel> _editTeamModelValidator;

        public TeamsController(
            ITeamService teamService,
            IValidator<CreateTeamModel> createTeamModelValidator,
            IValidator<EditTeamModel> editTeamModelValidator)
        {
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
            _createTeamModelValidator = createTeamModelValidator ?? throw new ArgumentNullException(nameof(createTeamModelValidator));
            _editTeamModelValidator = editTeamModelValidator ?? throw new ArgumentNullException(nameof(editTeamModelValidator));
        }

        [HttpGet]
        [Authorize(Policies.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetTeams()
        {
            return Ok(await _teamService.GetTeamsAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Policies.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetTeam(Guid id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            return team != null 
                ? Ok(team) 
                : NotFound();
        }

        [HttpPost]
        /*[Authorize(Policies.Admin)]*/
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateTeam([FromForm] CreateTeamModel сreateTeamModel)
        {
            var validationResult = await _createTeamModelValidator.ValidateAsync(сreateTeamModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToString());
            }

            await _teamService.CreateTeamAsync(сreateTeamModel);

            return Ok();
        }
        
        [HttpPut]
        [Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> EditTeam([FromForm] EditTeamModel team)
        {
            var validationResult = await _editTeamModelValidator.ValidateAsync(team);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage).First());
            }

            await _teamService.EditTeamAsync(team);

            return Ok();
        }
    }
}
