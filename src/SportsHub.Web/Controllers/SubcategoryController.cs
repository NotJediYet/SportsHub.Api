using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Business.Services.Abstraction;
using SportsHub.Shared.Models;
using SportsHub.Web.Security;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private readonly ISubcategoryService _subcategoryService;
        private readonly ICategoryService _categoryService;

        public SubcategoryController(ISubcategoryService subcategoryService, 
            ICategoryService categoryService)
        {
            _subcategoryService = subcategoryService;
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _subcategoryService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetSubcategoryAsync(Guid id)
        {
            var subcategory = await _subcategoryService.GetByIdAsync(id);

            return subcategory != null ? Ok(subcategory) : NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Admin)]
        public async Task<IActionResult> CreateSubcategoryAsync(
            CreateSubcategoryModel сreateSubcategoryModel)
        {
            var doesCategoryExist = await _categoryService
                .DoesCategoryAlredyExistByIdAsync(сreateSubcategoryModel.CategoryId);

            if (!doesCategoryExist)
            {
                return BadRequest("Category with that id doesn't exist!");
            }
            if (await _subcategoryService.DoesSubcategoryAlreadyExistByNameAsync(
                    сreateSubcategoryModel.Name))
            {
                return BadRequest("Subcategory with that name already exists!");
            }

            await _subcategoryService.CreateAsync(
                сreateSubcategoryModel.Name, сreateSubcategoryModel.CategoryId);

            return Ok();
        }
    }
}
