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
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguageService _languageService;
        private IValidator<CreateLanguageModel> _createLanguageModelValidator;
        private IValidator<EditLanguageModel> _editLanguageModelValidator;
        public LanguagesController(
            ILanguageService languageService,
            IValidator<CreateLanguageModel> createLanguageModelValidator,
            IValidator<EditLanguageModel> editLanguageModelValidator)
        {
            _languageService = languageService ?? throw new ArgumentNullException(nameof(languageService));
            _createLanguageModelValidator = createLanguageModelValidator ?? throw new ArgumentNullException(nameof(createLanguageModelValidator));
            _editLanguageModelValidator = editLanguageModelValidator ?? throw new ArgumentNullException(nameof(editLanguageModelValidator));
        }

        [HttpGet]
        [Authorize(Policies.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetLanguages()
        {
            return Ok(await _languageService.GetLanguagesAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Policies.User)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetLanguage(Guid id)
        {
            var category = await _languageService.GetLanguageByIdAsync(id);

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
        public async Task<IActionResult> CreateLanguage([FromForm]CreateLanguageModel сreateLanguageModel)
        {
            var validationResult = await _createLanguageModelValidator.ValidateAsync(сreateLanguageModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.ToString());
            }

            await _languageService.CreateLanguageAsync(сreateLanguageModel.Name, сreateLanguageModel.Code);

            return Ok();
        }

        [HttpPut]
        [Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> EditLanguage([FromForm]EditLanguageModel editLanguageModel)
        {
            var validationResult = await _editLanguageModelValidator.ValidateAsync(editLanguageModel);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(error => error.ErrorMessage).First());
            }

            await _languageService.EditLanguageAsync(editLanguageModel);

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteLanguage(Guid id)
        {
            var language = await _languageService.DeleteLanguageAsync(id);

            return language != null
                ? Ok(language)
                : NotFound();
        }
    }
}
