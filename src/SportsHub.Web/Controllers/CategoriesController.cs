using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Business.Services;
using SportsHub.Security;
using SportsHub.Shared.Models;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private IValidator<CreateCategoryModel> _createCategoryModelValidator;

        public CategoriesController(
            ICategoryService categoryService,
            IValidator<CreateCategoryModel> createCategoryModelValidator)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
            _createCategoryModelValidator = createCategoryModelValidator ?? throw new ArgumentNullException(nameof(createCategoryModelValidator));
        }

        [HttpGet]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetCategories()
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
        public async Task<IActionResult> CreateCategory(CreateCategoryModel сreateCategoryModel)
        {
            var validationResult = await _createCategoryModelValidator.ValidateAsync(сreateCategoryModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToString());
            }

            await _categoryService.CreateCategoryAsync(сreateCategoryModel.Name);

            return Ok();
        }
    }
}
