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
    public class FollowsController : ControllerBase
    {
        private readonly IFollowService _followService;

        public FollowsController(IFollowService followService)
        {
            _followService = followService;
        }

        [HttpPost("{followedAuthorId}")] // POST: api/Follows/{followedAuthorId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<string>> FollowAuthor(string followedAuthorId)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (authorId == null)
            {
                return Unauthorized("Author not authenticated");
            }

            var result = await _followService.FollowAuthorAsync(authorId, followedAuthorId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpDelete("{followedAuthorId}")] // DELETE: api/Follows/{followedAuthorId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<bool>> UnfollowAuthor(string followedAuthorId)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (authorId == null)
            {
                return Unauthorized("Author not authenticated");
            }

            var result = await _followService.UnfollowAuthorAsync(authorId, followedAuthorId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpGet("followers/{authorId}")] // GET: api/Follows/followers/{authorId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<IEnumerable<FollowerResponse>>> GetFollowersByAuthorId()
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _followService.GetFollowersByAuthorIdAsync(authorId);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpGet("following/{authorId}")] // GET: api/Follows/following/{authorId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<IEnumerable<FollowingResponse>>> GetFollowingByAuthorId()
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _followService.GetFollowingByAuthorIdAsync(authorId);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}

