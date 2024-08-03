using BlogAPI.DTOs.Request;
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
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost] // POST: api/Comments
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<string>> AddComment([FromBody] CommentRequest commentRequest)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            commentRequest.AuthorId = authorId;

            var result = await _commentService.AddCommentAsync(commentRequest);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.SuccessMessage);
        }

        [HttpPost("{parentCommentId}/replies")] // POST: api/Comments/{parentCommentId}/replies
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<string>> AddReply(long parentCommentId, [FromBody] CommentRequest commentRequest)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            commentRequest.AuthorId = authorId;

            var result = await _commentService.AddReplyAsync(parentCommentId, commentRequest);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.SuccessMessage);
        }

        [HttpGet("post/{postId}")] // GET: api/Comments/post/{postId}
        [Authorize(Roles = "Author, Admin")]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetCommentsByPostId(long postId)
        {
            var result = await _commentService.GetCommentsByPostIdAsync(postId);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpGet("{commentId}/replies")] // GET: api/Comments/{commentId}/replies
        [Authorize(Roles = "Author, Admin")]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetRepliesByCommentId(long commentId)
        {
            var result = await _commentService.GetRepliesByCommentIdAsync(commentId);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }
            return Ok(result.Data);
        }

        [HttpPut("{commentId}")] // PUT: api/Comments/{commentId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<string>> UpdateComment(long commentId, [FromBody] CommentRequest commentRequest)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            commentRequest.AuthorId = authorId;

            var result = await _commentService.UpdateCommentAsync(commentId, commentRequest);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.SuccessMessage);
        }

        [HttpDelete("{commentId}")] // DELETE: api/Comments/{commentId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<bool>> DeleteComment(long commentId)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _commentService.DeleteCommentAsync(authorId, commentId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Data);
        }
    }
}

