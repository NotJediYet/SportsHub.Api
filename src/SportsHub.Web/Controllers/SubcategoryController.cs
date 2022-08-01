using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Web.Interfaces;
using SportsHub.Web.Models;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : Controller
    {
        private readonly ISubcategoryService _subcategoryService;

        public SubcategoryController(ISubcategoryService subcategoryService)
        {
            _subcategoryService = subcategoryService;
        }

        [HttpGet("get")]
        [AllowAnonymous]
        public IEnumerable<Subcategory> GetSubcategories()
        {
            return _subcategoryService.GetSubcategories();
        }

        [HttpGet("get/{id}")]
        [AllowAnonymous]
        public IActionResult GetSubcategory(int id)
        {
            try
            {
                return Ok(_subcategoryService.GetSubcategoryByID(id));
            }
            catch (ApplicationException appEx)
            {
                return NotFound(appEx.Message);
            }
        }

        [HttpPost("add")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult CreateSubcategory(string newName, int categoryId)
        {
            try
            {
                _subcategoryService.CreateSubcategory(newName, categoryId);
            }
            catch (ApplicationException appEx)
            {
                return BadRequest(new { appEx.Message });
            }
            return RedirectToAction("GetSubcategories");
        }
    }
}