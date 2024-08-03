using BlogAPI.DTOs.Response;
using BlogAPI.Exceptions;
using System;
namespace BlogAPI.Services.Interfaces
{
	public interface IFollowService
	{
        Task<ServiceResult<string>> FollowAuthorAsync(string authorId, string followedAuthorId);
        Task<ServiceResult<bool>> UnfollowAuthorAsync(string authorId, string followedAuthorId);
        Task<ServiceResult<IEnumerable<FollowerResponse>>> GetFollowersByAuthorIdAsync(string authorId);
        Task<ServiceResult<IEnumerable<FollowingResponse>>> GetFollowingByAuthorIdAsync(string authorId);
    }
}

