using BlogAPI.DTOs.Request;
using BlogAPI.DTOs.Response;
using BlogAPI.Entities.Enums;
using BlogAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost] // POST: api/Posts
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<string>> AddPost([FromBody] PostRequest postRequest)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            postRequest.AuthorId = authorId;

            var result = await _postService.AddPostAsync(postRequest);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpPut("{postId}")] // PUT: api/Posts/{postId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<string>> UpdatePost(long postId, [FromBody] PostRequest postRequest)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            postRequest.AuthorId = authorId;

            var result = await _postService.UpdatePostAsync(postId, postRequest);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }
    
        [HttpGet("{postId}")] // GET: api/Posts/{postId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<PostResponse>> GetPostById(long postId)
        {
            var result = await _postService.GetPostByIdAsync(postId);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpGet("tag/{tagName}")] // GET: api/Posts/tag/{tagName}
        [Authorize(Roles = "Author, Admin")]
        public async Task<ActionResult<IEnumerable<PostResponse>>> GetPostsByTag(string tagName)
        {
            var result = await _postService.GetPostsByTagAsync(tagName);

            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpGet] // GET: api/Posts
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<PostResponse>>> GetAllPosts()
        {
            var result = await _postService.GetAllPostsAsync();
            if (!result.Success)
            {
                return Problem(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpGet("active")]
        [AllowAnonymous]
        public async Task<IActionResult> GetActivedPosts()
        {
            var result = await _postService.GetActivePostsAsync();
            return Ok(result.Data);
        }

        [HttpGet("banned")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetBannedPosts()
        {
            var result = await _postService.GetBannedPostsAsync();
            return Ok(result.Data);
        }

        [HttpGet("deactivated")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetDeactivatedPosts()
        {
            var result = await _postService.GetDeactivatedPostsAsync();
            return Ok(result.Data);
        }

        [HttpPatch("{postId}/ban")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BanPost(long postId)
        {
            var result = await _postService.BanPostAsync(postId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpPatch("{postId}/deactivate")]
        [Authorize(Roles = "Author, Admin")]
        public async Task<IActionResult> DeactivatePost(long postId)
        {
            var result = await _postService.DeactivatePostAsync(postId);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpPatch("{postId}/activate")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivatePost(long postId)
        {
            var result = await _postService.ActivatePostAsync(postId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpPost("{postId}/image")] // POST: api/Posts/{postId}/image
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<string>> AddImageToPost(long postId, IFormFile imageRequest)
        {
            var result = await _postService.AddImageToPostAsync(postId, imageRequest);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpDelete("{postId}/image")] // DELETE: api/Posts/{postId}/image
        [Authorize(Roles = "Author, Admin")]
        public async Task<ActionResult<bool>> RemoveImageFromPost(long postId)
        {
            var result = await _postService.RemoveImageFromPostAsync(postId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}

