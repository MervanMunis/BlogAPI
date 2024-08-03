using System;
using BlogAPI.DTOs.Request;
using BlogAPI.DTOs.Response;
using BlogAPI.Exceptions;

namespace BlogAPI.Services.Interfaces
{
	public interface IAuthorService
	{
        Task<ServiceResult<string>> CreateAuthorAsync(AuthorRequest authorRequest);

        Task<ServiceResult<string>> UpdateAuthorAsync(string authorId, AuthorRequest authorRequest);

        Task<ServiceResult<AuthorResponse>> GetAuthorByIdAsync(string authorId);

        Task<ServiceResult<AuthorResponse>> GetAuthorByUsernameAsync(string username);

        Task<ServiceResult<IEnumerable<AuthorResponse>>> GetAllAuthorsAsync();

        Task<ServiceResult<string>> DeactivateAuthorAsync(string userName);

        Task<ServiceResult<string>> ActivateAuthorAsync(string username);

        Task<ServiceResult<string>> BanAuthorAsync(string username);

        Task<ServiceResult<string>> AddProfileImageAsync(string authorId, IFormFile image);

        Task<ServiceResult<byte[]>> GetAuthorImageAsync(string authorId);

        Task<ServiceResult<bool>> RemoveProfileImageAsync(string authorId);

        Task<ServiceResult<string>> ForgotPasswordAsync(ForgotPasswordRequest forgotPasswordRequest);
        Task<ServiceResult<string>> ResetPasswordAsync(string token, string email, string newPassword);


        // New methods for forgot password
        Task<ServiceResult<string>> ForgotPasswordTokenAsync(ForgotPasswordRequest request);
        Task<ServiceResult<string>> ResetPasswordByTokenAsync(ResetPasswordRequest request);


    }
}

