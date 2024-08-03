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
    public class BookmarksController : ControllerBase
    {
        private readonly IBookmarkService _bookmarkService;

        public BookmarksController(IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        [HttpPost("{postId}")] // POST: api/Bookmarks/{postId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<string>> AddBookmark(long postId)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (authorId == null)
            {
                return Unauthorized("Author not authenticated");
            }

            var result = await _bookmarkService.AddBookmarkAsync(postId, authorId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpDelete("{postId}")] // DELETE: api/Bookmarks/{postId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<bool>> RemoveBookmark(long postId)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (authorId == null)
            {
                return Unauthorized("Author not authenticated");
            }

            var result = await _bookmarkService.RemoveBookmarkAsync(postId, authorId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpGet] // GET: api/Bookmarks/{authorId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<IEnumerable<BookmarkResponse>>> GetBookmarksByAuthorId()
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _bookmarkService.GetBookmarksByAuthorIdAsync(authorId);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}

