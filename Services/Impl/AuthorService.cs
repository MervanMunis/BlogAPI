using BlogAPI.Data;
using BlogAPI.DTOs.Request;
using BlogAPI.DTOs.Response;
using BlogAPI.Entities.Enums;
using BlogAPI.Entities.Models;
using BlogAPI.Exceptions;
using BlogAPI.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Services.Impl
{
    public class AuthorService : IAuthorService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IFileService _fileService;
        private readonly IEmailService _emailService;

        public AuthorService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IConfiguration configuration, IEmailService emailService, IFileService fileService)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
            _fileService = fileService;
            _emailService = emailService;
        }

        // Create a new author
        public async Task<ServiceResult<string>> CreateAuthorAsync(AuthorRequest authorRequest)
        {
            // Ensure the author is at least 16 years old
            if ((DateTime.Now.Year - authorRequest.BirthDate.Year) < 16)
            {
                return ServiceResult<string>.FailureResult("Author must be at least 16 years old.");
            }

            // Check if the author already exists
            var existingAuthor = await _userManager.Users
                .FirstOrDefaultAsync(a => a.Email == authorRequest.Email || a.UserName == authorRequest.UserName); 

            if (existingAuthor != null)
            {
                return ServiceResult<string>.FailureResult("The author already exists!");
            }

            // Assign UserName to Email if it's null
            if (string.IsNullOrEmpty(authorRequest.UserName))
            {
                authorRequest.UserName = authorRequest.Email;
            }

            // Assign DisplayName to UserName if it's null
            if (string.IsNullOrEmpty(authorRequest.DisplayName))
            {
                authorRequest.DisplayName = authorRequest.UserName.ToLower();
            }

            // Create a new ApplicationUser
            var user = new ApplicationUser
            {
                Email = authorRequest.Email,
                Name = authorRequest.Name,
                LastName = authorRequest.LastName,
                UserName = authorRequest.UserName,
                Gender = authorRequest.Gender!.ToString(),
                BirthDate = authorRequest.BirthDate
            };

            // Add the user to the database
            var result = await _userManager.CreateAsync(user, authorRequest.Password);

            if (!result.Succeeded)
            {
                return ServiceResult<string>.FailureResult("Failed to create author.");
            }

            // Add the user to the "Author" role
            await _userManager.AddToRoleAsync(user, "Author");

            var author = new Author
            {
                AuthorId = user.Id,
                DisplayName = authorRequest.DisplayName!,
                Bio = authorRequest.Bio,
                AuthorStatus = Status.Active.ToString()
            };

            await _context.Authors!.AddAsync(author);
            await _context.SaveChangesAsync();

            // Create an AuthorResponse DTO to return
            var authorResponse = new AuthorResponse
            {
                AuthorId = author.AuthorId,
                DisplayName = author.DisplayName,
                Bio = author.Bio,
                Name = user.Name,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Gender = author.ApplicationUser!.Gender,
                BirthDate = user.BirthDate ?? DateTime.MinValue,
                RegisterDate = user.RegisterDate,
                AuthorStatus = author.AuthorStatus
            };

            return ServiceResult<string>.SuccessMessageResult("Author is created.");
        }

        // Update an existing author
        public async Task<ServiceResult<string>> UpdateAuthorAsync(string authorId, AuthorRequest authorRequest)
        {
            var author = await _context.Authors!
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(a => a.AuthorId == authorId);

            if (author == null)
            {
                return ServiceResult<string>.FailureResult("Author not found.");
            }

            var user = author.ApplicationUser;

            if (user == null)
            {
                return ServiceResult<string>.FailureResult("Associated user not found.");
            }

            // Update the user and author details
            user.Name = authorRequest.Name;
            user.LastName = authorRequest.LastName;
            user.UserName = authorRequest.UserName;
            user.Email = authorRequest.Email;
            user.Gender = authorRequest.Gender!.ToString();
            user.BirthDate = authorRequest.BirthDate;

            author.DisplayName = authorRequest.DisplayName!;
            author.Bio = authorRequest.Bio;

            _context.Authors!.Update(author);
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return ServiceResult<string>.FailureResult("Failed to update author.");
            }

            await _context.SaveChangesAsync();

            // Create an AuthorResponse DTO to return
            var authorResponse = new AuthorResponse
            {
                AuthorId = author.AuthorId,
                DisplayName = author.DisplayName,
                Bio = author.Bio,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Gender = user.Gender,
                BirthDate = user.BirthDate ?? DateTime.MinValue,
                RegisterDate = user.RegisterDate,
                AuthorStatus = author.AuthorStatus
            };

            return ServiceResult<string>.SuccessMessageResult("The author is updated.");
        }

        // Get an author by their ID
        public async Task<ServiceResult<AuthorResponse>> GetAuthorByIdAsync(string authorId)
        {
            var author = await _context.Authors!
                .Where(a => a.AuthorStatus == Status.Active.ToString())
                .Include(a => a.ApplicationUser)
                .Select(a => new AuthorResponse
                {
                    AuthorId = a.AuthorId,
                    DisplayName = a.DisplayName,
                    Bio = a.Bio,
                    Name = a.ApplicationUser!.Name,
                    LastName = a.ApplicationUser!.LastName,
                    Email = a.ApplicationUser.Email,
                    Gender = a.ApplicationUser.Gender,
                    BirthDate = a.ApplicationUser.BirthDate ?? DateTime.MinValue,
                    RegisterDate = a.ApplicationUser.RegisterDate,
                    AuthorStatus = a.AuthorStatus
                })
                .FirstOrDefaultAsync(a => a.AuthorId == authorId);

            if (author == null)
            {
                return ServiceResult<AuthorResponse>.FailureResult("Author not found.");
            }

            return ServiceResult<AuthorResponse>.SuccessResult(author);
        }

        // Get an author by their username
        public async Task<ServiceResult<AuthorResponse>> GetAuthorByUsernameAsync(string username)
        {
            var author = await _context.Authors!
                .Include(a => a.ApplicationUser)
                .Where(a => a.AuthorStatus == Status.Active.ToString())
                .Select(a => new AuthorResponse
                {
                    AuthorId = a.AuthorId,
                    DisplayName = a.DisplayName,
                    Bio = a.Bio,
                    Name = a.ApplicationUser!.Name,
                    LastName = a.ApplicationUser!.LastName,
                    Email = a.ApplicationUser.Email,
                    UserName = a.ApplicationUser.UserName,
                    Gender = a.ApplicationUser.Gender,
                    BirthDate = a.ApplicationUser.BirthDate ?? DateTime.MinValue,
                    RegisterDate = a.ApplicationUser.RegisterDate,
                    AuthorStatus = a.AuthorStatus
                })
                .FirstOrDefaultAsync(a => a.UserName == username);

            if (author == null)
            {
                return ServiceResult<AuthorResponse>.FailureResult("Author not found.");
            }

            return ServiceResult<AuthorResponse>.SuccessResult(author);
        }

        // Get all active authors
        public async Task<ServiceResult<IEnumerable<AuthorResponse>>> GetAllAuthorsAsync()
        {
            // Retrieve all active authors from the database
            var authors = await _context.Authors!
                .Where(a => a.AuthorStatus == Status.Active.ToString())
                .Include(a => a.ApplicationUser)
                .Select(a => new AuthorResponse
                {
                    AuthorId = a.AuthorId,
                    DisplayName = a.DisplayName,
                    Bio = a.Bio,
                    Name = a.ApplicationUser!.Name,
                    LastName = a.ApplicationUser!.LastName,
                    UserName = a.ApplicationUser!.UserName,
                    Email = a.ApplicationUser.Email,
                    Gender = a.ApplicationUser.Gender,
                    BirthDate = a.ApplicationUser.BirthDate ?? DateTime.MinValue,
                    RegisterDate = a.ApplicationUser.RegisterDate,
                    AuthorStatus = a.AuthorStatus
                })
                .ToListAsync();

            if (authors == null)
            {
                return ServiceResult<IEnumerable<AuthorResponse>>.FailureResult("Authors not found.");
            }

            return ServiceResult<IEnumerable<AuthorResponse>>.SuccessResult(authors);
        }

        // Deactivate an author
        public async Task<ServiceResult<string>> DeactivateAuthorAsync(string username)
        {
            var author = await _context.Authors!
                .FirstOrDefaultAsync(a => a.ApplicationUser!.UserName == username);
            if (author == null)
            {
                return ServiceResult<string>.FailureResult("Author not found.");
            }

            // Set the author status to inactive
            author.AuthorStatus = Status.InActive.ToString();
            _context.Authors!.Update(author);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("The author is deactivated.");
        }

        // Activate an author
        public async Task<ServiceResult<string>> ActivateAuthorAsync(string username)
        {
            var author = await _context.Authors!
                .FirstOrDefaultAsync(a => a.ApplicationUser!.UserName == username);

            if (author == null)
            {
                return ServiceResult<string>.FailureResult("Author not found.");
            }

            author.AuthorStatus = Status.Active.ToString();
            _context.Authors!.Update(author);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("The author is activated.");
        }

        // Ban an author
        public async Task<ServiceResult<string>> BanAuthorAsync(string username)
        {
            var author = await _context.Authors!.FirstOrDefaultAsync(a => a.ApplicationUser!.UserName == username);
            if (author == null)
            {
                return ServiceResult<string>.FailureResult("Author not found.");
            }

            author.AuthorStatus = Status.Banned.ToString();
            _context.Authors!.Update(author);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessResult("The author is banned.");
        }

        // Add a profile image for an author
        public async Task<ServiceResult<string>> AddProfileImageAsync(string authorId, IFormFile image)
        {
            // Find the author in the database
            var author = await _context.Authors!
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(a => a.AuthorId == authorId);

            if (author == null)
            {
                return ServiceResult<string>.FailureResult("Author not found.");
            }

            // Delete the old profile picture if it exists
            if (author.ProfilePicture != null)
            {
                await _fileService.DeleteFileAsync(author.ProfilePicture);  
            }

            // Save the new profile picture
            var filePath = await _fileService.SaveFileAsync(image, "AuthorImages");
            author.ProfilePicture = filePath;

            _context.Authors!.Update(author);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Profile image added successfully.");
        }

        // Retrieve the profile image of an author by author ID
        public async Task<ServiceResult<byte[]>> GetAuthorImageAsync(string authorId)
        {
            try
            {
                var imageBytes = await _fileService.GetImageByAuthorIdAsync(authorId);
                return ServiceResult<byte[]>.SuccessResult(imageBytes);
            }
            catch (FileNotFoundException ex)
            {
                return ServiceResult<byte[]>.FailureResult(ex.Message);
            }
        }

        // Remove the profile image of an author
        public async Task<ServiceResult<bool>> RemoveProfileImageAsync(string authorId)
        {
            var author = await _context.Authors!
                .Include(a => a.ApplicationUser)
                .FirstOrDefaultAsync(a => a.AuthorId == authorId);

            if (author == null)
            {
                return ServiceResult<bool>.FailureResult("Author not found.");
            }

            // Delete the profile picture if it exists
            if (author.ProfilePicture != null)
            {
                await _fileService.DeleteFileAsync(author.ProfilePicture);
                author.ProfilePicture = null;
                _context.Authors!.Update(author);
                await _context.SaveChangesAsync();
            }

            return ServiceResult<bool>.SuccessResult(true);
        }

        public async Task<ServiceResult<string>> ForgotPasswordTokenAsync(ForgotPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return ServiceResult<string>.FailureResult("User not found.");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            // Here we need to send the token via email to the user
            // For simplicity, we're returning the token as a response (do not do this in a real application)
            return ServiceResult<string>.SuccessResult(token);
        }

        public async Task<ServiceResult<string>> ResetPasswordByTokenAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return ServiceResult<string>.FailureResult("User not found.");
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
            {
                return ServiceResult<string>.FailureResult("Failed to reset password.");
            }

            return ServiceResult<string>.SuccessResult("Password reset successfully.");
        }

    }
}

