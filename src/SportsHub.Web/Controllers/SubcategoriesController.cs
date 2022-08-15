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
    public class SubcategoriesController : ControllerBase
    {
        private readonly ISubcategoryService _subcategoryService;
        private IValidator<CreateSubcategoryModel> _createSubcategoryModelValidator;

        public SubcategoriesController(
            ISubcategoryService subcategoryService,
            IValidator<CreateSubcategoryModel> createSubcategoryModelValidator)
        {
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));
            _createSubcategoryModelValidator = createSubcategoryModelValidator ?? throw new ArgumentNullException(nameof(createSubcategoryModelValidator));
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
        /*[Authorize(Policies.Admin)]*/
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateSubcategory(CreateSubcategoryModel сreateSubcategoryModel)
        {
            var validationResult = await _createSubcategoryModelValidator.ValidateAsync(сreateSubcategoryModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToString());
            }

            await _subcategoryService.CreateSubcategoryAsync(сreateSubcategoryModel.Name, сreateSubcategoryModel.CategoryId);

            return Ok();
        }
    }
}
