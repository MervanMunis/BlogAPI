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
    public class SubCategoriesController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoriesController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        [HttpGet] // GET: api/SubCategories
        public async Task<ActionResult<IEnumerable<SubCategoryResponse>>> GetSubCategories()
        {
            var result = await _subCategoryService.GetAllSubCategoriesAsync();
            if (!result.Success)
            {
                return Problem(result.ErrorMessage);
            }
            return Ok(result.Data);
        }
        [HttpGet("{id}")] // GET: api/SubCategories/5
        public async Task<ActionResult<SubCategoryResponse>> GetSubCategory(short id)
        {
            var result = await _subCategoryService.GetSubCategoryByIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpGet("{id}/posts")] // GET: api/SubCategories/5/posts
        public async Task<ActionResult<IEnumerable<PostResponse>>> GetPostsBySubCategory(short id)
        {
            var result = await _subCategoryService.GetPostsBySubCategoryIdAsync(id);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpPost] // POST: api/SubCategories
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> PostSubCategory([FromBody] SubCategoryRequest subCategoryRequest)
        {
            var result = await _subCategoryService.AddSubCategoryAsync(subCategoryRequest);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("The subcategory is successfully created.");
        }

        [HttpPut("{id}")] // PUT: api/SubCategories/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> PutSubCategory(short id, [FromBody] SubCategoryRequest subCategoryRequest)
        {
            var result = await _subCategoryService.UpdateSubCategoryAsync(id, subCategoryRequest);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("The subcategory is updated successfully.");
        }

        [HttpPatch("{id}/status/inactive")] // PATCH: api/SubCategories/5/status/inactive
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> InActiveSubCategory(short id)
        {
            var result = await _subCategoryService.SetSubCategoryStatusAsync(id, Status.InActive.ToString());
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok("The subcategory is set to inactive.");
        }

        [HttpPatch("{id}/status/active")] // PATCH: api/SubCategories/5/status/active
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<string>> ActiveSubCategory(short id)
        {
            var result = await _subCategoryService.SetSubCategoryStatusAsync(id, Status.Active.ToString());
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok("The subcategory is set to active.");
        }
    }
}

