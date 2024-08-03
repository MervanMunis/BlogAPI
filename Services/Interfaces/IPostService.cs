using BlogAPI.DTOs.Request;
using BlogAPI.DTOs.Response;
using BlogAPI.Exceptions;
using System;
namespace BlogAPI.Services.Interfaces
{
	public interface IPostService
	{
        Task<ServiceResult<string>> AddPostAsync(PostRequest postRequest);
        Task<ServiceResult<string>> UpdatePostAsync(long postId, PostRequest postRequest);
        Task<ServiceResult<PostResponse>> GetPostByIdAsync(long postId);
        Task<ServiceResult<IEnumerable<PostResponse>>> GetActivePostsAsync();
        Task<ServiceResult<IEnumerable<TagedPostResponse>>> GetPostsByTagAsync(string tagName);
        Task<ServiceResult<string>> BanPostAsync(long postId);
        Task<ServiceResult<string>> DeactivatePostAsync(long postId);
        Task<ServiceResult<string>> ActivatePostAsync(long postId);
        Task<ServiceResult<IEnumerable<PostResponse>>> GetAllPostsAsync();
        Task<ServiceResult<IEnumerable<PostResponse>>> GetBannedPostsAsync();
        Task<ServiceResult<IEnumerable<PostResponse>>> GetDeactivatedPostsAsync();
        Task<ServiceResult<string>> AddImageToPostAsync(long postId, IFormFile image);
        Task<ServiceResult<bool>> RemoveImageFromPostAsync(long postId);
    }
}

