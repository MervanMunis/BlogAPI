using BlogAPI.DTOs.Response;
using BlogAPI.Exceptions;
using System;
namespace BlogAPI.Services.Interfaces
{
	public interface ILikeService
	{
        Task<ServiceResult<string>> LikePostAsync(long postId, string authorId);
        Task<ServiceResult<bool>> UnlikePostAsync(long postId, string authorId);
    }
}

