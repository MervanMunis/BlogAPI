using BlogAPI.DTOs.Response;
using BlogAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly ILikeService _likeService;

        public LikesController(ILikeService likeService)
        {
            _likeService = likeService;
        }

        [HttpPost("{postId}")] // POST: api/Likes/{postId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<string>> LikePost(long postId)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (authorId == null)
            {
                return Unauthorized("Author not authenticated");
            }

            var result = await _likeService.LikePostAsync(postId, authorId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpDelete("{postId}")] // DELETE: api/Likes/{postId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<bool>> UnlikePost(long postId)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (authorId == null)
            {
                return Unauthorized("Author not authenticated");
            }

            var result = await _likeService.UnlikePostAsync(postId, authorId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}

