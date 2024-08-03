using System.Security.Claims;
using BlogAPI.DTOs.Response;
using BlogAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentLikesController : ControllerBase
    {
        private readonly ICommentLikeService _commentLikeService;

        public CommentLikesController(ICommentLikeService commentLikeService)
        {
            _commentLikeService = commentLikeService;
        }

        [HttpPost("{commentId}")] // POST: api/CommentLikes/{commentId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<string>> LikeComment(long commentId)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (authorId == null)
            {
                return Unauthorized("Author not authenticated");
            }

            var result = await _commentLikeService.LikeCommentAsync(commentId, authorId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpDelete("{commentId}")] // DELETE: api/CommentLikes/{commentId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<bool>> UnlikeComment(long commentId)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (authorId == null)
            {
                return Unauthorized("Author not authenticated");
            }

            var result = await _commentLikeService.UnlikeCommentAsync(commentId, authorId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpGet("{commentId}")] // GET: api/CommentLikes/{commentId}
        public async Task<ActionResult<IEnumerable<CommentLikeResponse>>> GetLikesByCommentId(long commentId)
        {
            var result = await _commentLikeService.GetLikesByCommentIdAsync(commentId);

            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}
