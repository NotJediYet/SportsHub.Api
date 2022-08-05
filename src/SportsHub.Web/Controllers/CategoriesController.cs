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
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await _categoryService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetCategory(Guid id)
        {
            var category = await _categoryService.GetByIdAsync(id);

            return category != null ? Ok(category) : NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Admin)]
        public async Task<IActionResult> CreateCategory(
            CreateCategoryModel сreateCategoryModel)
        {
            var doesCategoryExist = await _categoryService
                .DoesCategoryAlreadyExistByNameAsync(сreateCategoryModel.Name);
            if (doesCategoryExist)
            {
                return BadRequest("Category with that name already exists!");
            }

            await _categoryService.CreateAsync(сreateCategoryModel.Name);

            return Ok();
        }
    }
}
