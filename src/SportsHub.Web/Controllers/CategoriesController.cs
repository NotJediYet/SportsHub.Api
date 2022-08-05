using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Security;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpGet]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await _categoryService.GetCategoriesAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);

            return category != null 
                ? Ok(category) 
                : NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateCategory(
            CreateCategoryModel сreateCategoryModel)
        {
            var doesCategoryExist = await _categoryService.DoesCategoryAlreadyExistByNameAsync(сreateCategoryModel.Name);
            if (doesCategoryExist)
            {
                return BadRequest("Category with that name already exists!");
            }

            await _categoryService.CreateCategoryAsync(сreateCategoryModel.Name);

            return Ok();
        }
    }
}
