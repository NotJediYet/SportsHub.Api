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

        public SubcategoryController(ISubcategoryService SubcategoryService)
        {
            _subcategoryService = SubcategoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _subcategoryService.GetAllAsync());
        }

        [HttpGet("{Id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSubcategoryAsync(Guid Id)
        {
            var Subcategory = await _subcategoryService.GetByIDAsync(Id);
            return Subcategory != null ? Ok(Subcategory) : NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Admin)]
        public async Task<IActionResult> CreateSubcategoryAsync(Subcategory Subcategory)
        {
            if (await _subcategoryService.CheckIfCategoryIdNotExists(
                Subcategory.CategoryId))
            {
                return BadRequest("Category with that id doesn't exist!");
            }
            if ((Subcategory.Id != Guid.Empty) && 
                (await _subcategoryService.CheckIfNameNotUniqueAsync(Subcategory.Name)))
            {
                return BadRequest("Subcategory with that id already exists!");
            }
            if (await _subcategoryService.CheckIfNameNotUniqueAsync(Subcategory.Name))
            {
                return BadRequest("Subcategory with that name already exists!");
            }
            await _subcategoryService.CreateAsync(Subcategory);
            return Ok();
        }
    }
}