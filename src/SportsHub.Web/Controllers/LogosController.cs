using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Business.Services;
using SportsHub.Shared.Models;
using SportsHub.Security;
using FluentValidation;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogosController : ControllerBase
    {
        private readonly ILogoService _logoService;
        private IValidator<CreateLogoModel> _createLogoModelValidator;

        public LogosController(
            ILogoService logoService,
            IValidator<CreateLogoModel> createLogoModelValidator)
        {
            _logoService = logoService ?? throw new ArgumentNullException(nameof(logoService));
            _createLogoModelValidator = createLogoModelValidator ?? throw new ArgumentNullException(nameof(createLogoModelValidator));
        }

        [HttpGet]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetLogos()
        {
            return Ok(await _logoService.GetLogosAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetLogo(Guid id)
        {
            var logo = await _logoService.GetLogoByIdAsync(id);
            return logo != null
                ? Ok(logo)
                : NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]

        public async Task<IActionResult> CreateLogo([FromForm] CreateLogoModel сreateLogoModel)
        {
            var result = await _createLogoModelValidator.ValidateAsync(сreateLogoModel);
            if (!result.IsValid)
            {
                return BadRequest(result.Errors.Select(e => e.ErrorMessage));
            }

            await _logoService.CreateLogoAsync(сreateLogoModel.Bytes, сreateLogoModel.UploadDate,
                                               сreateLogoModel.FileExtension, сreateLogoModel.TeamId);

            return Ok();
        }
    }
}
