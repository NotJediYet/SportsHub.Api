using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Business.Services.Abstraction;
using SportsHub.Shared.Models;
using SportsHub.Shared.Entities;
using SportsHub.Web.Security;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _categoryService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetCategoryAsync(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            return category != null ? Ok(category) : NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Admin)]
        public async Task<IActionResult> CreateCategoryAsync(
            CreateCategoryModel newCategory)
        {
            if (
                await _categoryService.CheckIfNameNotUniqueAsync(
                    newCategory.Name))
            {
                return BadRequest("Category with that name already exists!");
            }

            await _categoryService.CreateAsync(newCategory.Name);

            return Ok();
        }
    }
}
