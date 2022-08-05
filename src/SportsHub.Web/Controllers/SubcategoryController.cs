﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SportsHub.Business.Services.Abstraction;
using SportsHub.Shared.Models;
using SportsHub.Web.Security;

namespace SportsHub.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoriesController : ControllerBase
    {
        private readonly ISubcategoryService _subcategoryService;
        private readonly ICategoryService _categoryService;

        public SubcategoriesController(ISubcategoryService subcategoryService, 
            ICategoryService categoryService)
        {
            _subcategoryService = subcategoryService;
            _categoryService = categoryService;
        }

        [HttpGet]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetAllSubcategories()
        {
            return Ok(await _subcategoryService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [Authorize(Policies.User)]
        public async Task<IActionResult> GetSubcategory(Guid id)
        {
            var subcategory = await _subcategoryService.GetByIdAsync(id);

            return subcategory != null ? Ok(subcategory) : NotFound();
        }

        [HttpPost]
        [Authorize(Policies.Admin)]
        public async Task<IActionResult> CreateSubcategory(CreateSubcategoryModel сreateSubcategoryModel)
        {
            var doesCategoryExist = await _categoryService
                .DoesCategoryAlredyExistByIdAsync(сreateSubcategoryModel.CategoryId);
            if (!doesCategoryExist)
            {
                return BadRequest("Category with that id doesn't exist!");
            }

            var doesSubcategoryExist = await _subcategoryService
                .DoesSubcategoryAlreadyExistByNameAsync(сreateSubcategoryModel.Name);
            if (doesSubcategoryExist)
            {
                return BadRequest("Subcategory with that name already exists!");
            }

            await _subcategoryService.CreateAsync(сreateSubcategoryModel.Name,
                сreateSubcategoryModel.CategoryId);

            return Ok();
        }
    }
}
