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
    public class ReadingListsController : ControllerBase
    {
        private readonly IReadingListService _readingListService;

        public ReadingListsController(IReadingListService readingListService)
        {
            _readingListService = readingListService;
        }

        [HttpPost("{postId}")] // POST: api/ReadingLists/{postId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<string>> AddToReadingList(long postId)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (authorId == null)
            {
                return Unauthorized("Author not authenticated");
            }

            var result = await _readingListService.AddToReadingListAsync(postId, authorId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpDelete("{postId}")] // DELETE: api/ReadingLists/{postId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<bool>> RemoveFromReadingList(long postId)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (authorId == null)
            {
                return Unauthorized("Author not authenticated");
            }

            var result = await _readingListService.RemoveFromReadingListAsync(postId, authorId);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpGet] // GET: api/ReadingLists/{authorId}
        [Authorize(Roles = "Author")]
        public async Task<ActionResult<IEnumerable<ReadingListResponse>>> GetReadingListByAuthorId()
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _readingListService.GetReadingListByAuthorIdAsync(authorId);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
    }
}

