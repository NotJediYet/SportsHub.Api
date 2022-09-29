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
        private IValidator<EditSubcategoryModel> _editSubcategoryModelValidator;

        public SubcategoriesController(
            ISubcategoryService subcategoryService,
            IValidator<CreateSubcategoryModel> createSubcategoryModelValidator,
            IValidator<EditSubcategoryModel> editSubcategoryModelValidator)
        {
            _subcategoryService = subcategoryService ?? throw new ArgumentNullException(nameof(subcategoryService));
            _createSubcategoryModelValidator = createSubcategoryModelValidator ?? throw new ArgumentNullException(nameof(createSubcategoryModelValidator));
            _editSubcategoryModelValidator = editSubcategoryModelValidator ?? throw new ArgumentNullException(nameof(editSubcategoryModelValidator));
        }

        [HttpGet]
        [Authorize(Policies.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetSubcategories()
        {
            var subcategories = await _subcategoryService.GetSubcategoriesAsync();

            return Ok(subcategories);
        }

        [HttpGet("{id}")]
        [Authorize(Policies.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
            var validationResult = await _createSubcategoryModelValidator.ValidateAsync(сreateSubcategoryModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToString());
            }

            await _subcategoryService.CreateSubcategoryAsync(сreateSubcategoryModel.Name, сreateSubcategoryModel.CategoryId);

            return Ok();
        }

        [HttpPut]
        [Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> EditSubcategory(EditSubcategoryModel editSubcategoryModel)
        {
            var validationResult = await _editSubcategoryModelValidator.ValidateAsync(editSubcategoryModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage).First());
            }

            await _subcategoryService.EditSubcategoryAsync(editSubcategoryModel);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteSubcategory(Guid id)
        {
            var subcategory = await _subcategoryService.DeleteSubcategoryAsync(id);

            return subcategory != null
                ? Ok(subcategory)
                : NotFound();
        }
    }
}
