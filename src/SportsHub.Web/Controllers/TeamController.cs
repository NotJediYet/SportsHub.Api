using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Web.Interfaces;
using SportsHub.Web.Models;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : Controller
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpGet("get")]
        [AllowAnonymous]
        public IEnumerable<Team> GetTeams()
        {
            return _teamService.GetTeams();
        }

        [HttpGet("get/{id}")]
        [AllowAnonymous]
        public IActionResult GetTeam(int id)
        {
            try
            {
                return Ok(_teamService.GetTeamByID(id));
            }
            catch (ApplicationException appEx)
            {
                return NotFound(appEx.Message);
            }
        }

        [HttpPost("add")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult CreateTeam(string newName, int subcategoryId)
        {
            try
            {
                _teamService.CreateTeam                                                                                                                                                                                                                                                                                                                                                         (newName, subcategoryId);
            }
            catch (ApplicationException appEx)
            {
                return BadRequest(new { appEx.Message });
            }
            return RedirectToAction("GetTeams");
        }
    }
}
