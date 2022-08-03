using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Business.Services.Abstraction;
using SportsHub.Shared.Models;
using SportsHub.Web.Security;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService CategoryService)
        {
            _categoryService = CategoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _categoryService.GetAllAsync());
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategoryAsync(Guid Id)
        {
            var Category = await _categoryService.GetByIDAsync(Id);
            return Category != null ? Ok(Category) : NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Admin)]
        public async Task<IActionResult> CreateCategoryAsync(Category Category)
        {
            if ((Category.Id != Guid.Empty) && (await _categoryService.CheckIfNameNotUniqueAsync(Category.Name)))
            {
                return BadRequest("Category with that id already exists!");
            }
            if (await _categoryService.CheckIfNameNotUniqueAsync(Category.Name))
            {
                return BadRequest("Category with that name already exists!");
            }
            await _categoryService.CreateAsync(Category);
            return Ok();
        }
    }
}
