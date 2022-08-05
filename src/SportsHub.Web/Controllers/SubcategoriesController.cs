using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Security;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoriesController : ControllerBase
    {
        private readonly ISubcategoryService _subcategoryService;
        private readonly ICategoryService _categoryService;

        public SubcategoriesController(
            ISubcategoryService subcategoryService,
            ICategoryService categoryService)
        {
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpGet]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetSubcategories()
        {
            var subcategories = await _subcategoryService.GetSubcategoriesAsync();

            return Ok(subcategories);
        }

        [HttpGet("{id}")]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetSubcategory(Guid id)
        {
            var subcategory = await _subcategoryService.GetSubcategoryByIdAsync(id);

            return subcategory != null
                ? Ok(subcategory)
                : NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateSubcategory(CreateSubcategoryModel сreateSubcategoryModel)
        {
            var doesCategoryExist = await _categoryService.DoesCategoryAlredyExistByIdAsync(сreateSubcategoryModel.CategoryId);
            if (!doesCategoryExist)
            {
                return BadRequest("Category with that id doesn't exist!");
            }

            var doesSubcategoryExist = await _subcategoryService.DoesSubcategoryAlreadyExistByNameAsync(сreateSubcategoryModel.Name);
            if (doesSubcategoryExist)
            {
                return BadRequest("Subcategory with that name already exists!");
            }

            await _subcategoryService.CreateSubcategoryAsync(сreateSubcategoryModel.Name, сreateSubcategoryModel.CategoryId);

            return Ok();
        }
    }
}
