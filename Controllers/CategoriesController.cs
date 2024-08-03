    using BlogAPI.DTOs.Request;
using BlogAPI.DTOs.Response;
using BlogAPI.Entities.Enums;
using BlogAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet] // GET: api/Categories
        public async Task<ActionResult<IEnumerable<CategoryResponse>>> GetCategories()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            if (!result.Success)
            {
                return Problem(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}")] // GET: api/Categories/5
        public async Task<ActionResult<CategoryResponse>> GetCategory(short id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpPost] // POST: api/Categories
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> PostCategory([FromBody] CategoryRequest categoryRequest)
        {
            var result = await _categoryService.AddCategoryAsync(categoryRequest);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("The category is successfully created.");
        }

        [HttpPut("{id}")] // PUT: api/Categories/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> PutCategory(short id, [FromBody] CategoryRequest categoryRequest)
        {
            var result = await _categoryService.UpdateCategoryAsync(id, categoryRequest);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("The category is updated successfully.");
        }

        [HttpPatch("{id}/status/inactive")] // PATCH: api/Categories/5/status/inactive
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> InActiveCategory(short id)
        {
            var result = await _categoryService.SetCategoryStatusAsync(id, Status.InActive.ToString());
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok("The category is set to inactive.");
        }

        [HttpPatch("{id}/status/active")] // PATCH: api/Categories/5/status/active
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> SetCategoryActiveStatus(short id)
        {
            var result = await _categoryService.SetCategoryStatusAsync(id, Status.Active.ToString());
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("The category is set to active.");
        }
    }
}

