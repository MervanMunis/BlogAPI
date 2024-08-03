using System;
using System.Security.Claims;
using BlogAPI.DTOs.Request;
using BlogAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorRequest authorRequest)
        {
            var result = await _authorService.CreateAuthorAsync(authorRequest);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpPut]
        [Authorize(Roles = "Author")]
        public async Task<IActionResult> UpdateAuthor([FromBody] AuthorRequest authorRequest)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _authorService.UpdateAuthorAsync(authorId, authorRequest);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpGet("byId")]
        [Authorize(Roles = "Author")]
        public async Task<IActionResult> GetAuthorById()
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _authorService.GetAuthorByIdAsync(authorId);
            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpGet("username")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuthorByUsername(string userName)
        {
            var result = await _authorService.GetAuthorByUsernameAsync(userName);

            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return Ok(result.Data);
        }

        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllAuthors()
        {
            var result = await _authorService.GetAllAuthorsAsync();
            return Ok(result.Data);
        }

        [HttpPatch("/deactivate")]
        [Authorize(Roles = "Author, Admin")]
        public async Task<IActionResult> DeactivateAuthor(string username)
        {
            var result = await _authorService.DeactivateAuthorAsync(username);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpPatch("/activate")]
        [Authorize(Roles = "Author, Admin")]
        public async Task<IActionResult> ActivateAuthor(string username)
        {
            var result = await _authorService.ActivateAuthorAsync(username);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpPatch("/ban")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BanAuthor(string username)
        {
            var result = await _authorService.BanAuthorAsync(username);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpPost("add/profile-image")]
        [Authorize(Roles = "Author")]
        public async Task<IActionResult> AddProfileImage(IFormFile image)
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _authorService.AddProfileImageAsync(authorId, image);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }

        [HttpDelete("remove/profile-image")]
        [Authorize(Roles = "Author, Admin")]
        public async Task<IActionResult> RemoveProfileImage()
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _authorService.RemoveProfileImageAsync(authorId);

            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data);
        }
        
        /// <summary>
        /// Retrieves the image of an author by author ID.
        /// </summary>
        /// <param name="authorId">The ID of the author.</param>
        /// <returns>The image as a byte array.</returns>
        [HttpGet("image")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAuthorImage()
        {
            var authorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _authorService.GetAuthorImageAsync(authorId);

            if (!result.Success)
            {
                return NotFound(result.ErrorMessage);
            }

            return File(result.Data!, "image/jpeg"); // Assuming the images are JPEG. Adjust as necessary.
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPasswordToken([FromBody] ForgotPasswordRequest request)
        {
            var result = await _authorService.ForgotPasswordTokenAsync(request);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.Data); // You should ideally send an email with the token
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPasswordToken([FromBody] ResetPasswordRequest request)
        {
            var result = await _authorService.ResetPasswordByTokenAsync(request);
            if (!result.Success)
            {
                return BadRequest(result.ErrorMessage);
            }

            return Ok(result.SuccessMessage);
        }
    }
}

