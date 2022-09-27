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
        private readonly ISubcategoryService _subcategoryService;
        private readonly ICategoryService _categoryService;
        private IValidator<CreateTeamModel> _createTeamModelValidator;
        private IValidator<EditTeamModel> _editTeamModelValidator;

        public TeamsController(
            ITeamService teamService,
            ISubcategoryService subcategoryService,
            ICategoryService categoryService,
            IValidator<CreateTeamModel> createTeamModelValidator,
            IValidator<EditTeamModel> editTeamModelValidator)
        {
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
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
        public async Task<IActionResult> EditTeam([FromForm] EditTeamModel editTeamModel)
        {
            var validationResult = await _editTeamModelValidator.ValidateAsync(editTeamModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage).First());
            }

            await _teamService.EditTeamAsync(editTeamModel);

            return Ok();
        }

        [HttpGet("{location}/{categoryName}/{subcategoryName}")]
        /*[Authorize(Policies.Admin)]*/
        [AllowAnonymous]
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
                var subcategories = await _subcategoryService.GetByCategoryIdAsync(idCategory);

                teams = _teamService.GetTeamsFilteredBySubcategoryIds(subcategories, teams.ToList());
            }

            if (location != "All")
            {
                teams = _teamService.GetTeamsFilteredByLocation(location, teams.ToList());
            }

            if (subcategoryName != "All")
            {
                var idSubcategory = await _subcategoryService.FindSubcategoryIdBySubcategoryNameAsync(subcategoryName);

                teams = _teamService.GetTeamsFilteredBySubcategoryId(idSubcategory, teams.ToList());
            }

            return teams != null
                ? Ok(teams)
                : NotFound();
        }
    }
}
