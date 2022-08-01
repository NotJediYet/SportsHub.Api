using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Web.Interfaces;
using SportsHub.Web.Models;
using SportsHub.Web.Repositories;
using System.Data;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService; 
        }

        [HttpGet("get")]
        [AllowAnonymous]
        public IEnumerable<Category> GetCategories()
        {
            return _categoryService.GetCategories();
        }

        [HttpGet("get/{id}")]
        [AllowAnonymous]
        public IActionResult GetCategory(int id)
        {
            try
            {
                return Ok(_categoryService.GetCategoryByID(id));
            }
            catch (ApplicationException appEx)
            {
                return NotFound(appEx.Message);
            }
        }

        [HttpPost("add")]
        [Authorize(Policy = "AdminOnly")]
        public IActionResult CreateCategory(string newName)
        {
            try
            {
                _categoryService.CreateCategory(newName);
            }
            catch (ApplicationException appEx)
            {
                return BadRequest(new { appEx.Message });
            }
            return RedirectToAction("GetCategories");
        }
    }
}
