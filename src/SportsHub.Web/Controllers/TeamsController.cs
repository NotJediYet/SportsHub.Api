﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Security;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamsController : ControllerBase
    {
        private readonly ITeamService _teamService;
        private readonly ISubcategoryService _subcategoryService;

        public TeamsController(
            ITeamService teamService,
            ISubcategoryService subcategoryService)
        {
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));
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
            var doesSubcategoryExist = await _subcategoryService.DoesSubcategoryAlredyExistByIdAsync(сreateTeamModel.SubcategoryId);
            if (!doesSubcategoryExist)
            {
                return BadRequest("Subcategory with that id doesn't exist!");
            }

            var doesTeamExist = await _teamService.DoesTeamAlreadyExistByNameAsync(сreateTeamModel.Name);
            if (doesTeamExist)
            {
                return BadRequest("Team with that name already exists!");
            }

            await _teamService.CreateTeamAsync(сreateTeamModel.Name, сreateTeamModel.SubcategoryId);

            return Ok();
        }
    }
}
