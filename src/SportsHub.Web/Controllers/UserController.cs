using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace SportsHub.Web.Controllers
{

    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        [Authorize]
        [HttpGet("secured-get")]
        public IActionResult TestSecuredGet()
        {
            return Ok(new { Massage = "Here is some private info!" });
        }
    }
}