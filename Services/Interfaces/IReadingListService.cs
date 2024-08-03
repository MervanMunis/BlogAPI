using BlogAPI.DTOs.Response;
using BlogAPI.Exceptions;
using System;
namespace BlogAPI.Services.Interfaces
{
	public interface IReadingListService
	{
        Task<ServiceResult<string>> AddToReadingListAsync(long postId, string authorId);
        Task<ServiceResult<bool>> RemoveFromReadingListAsync(long postId, string authorId);
        Task<ServiceResult<IEnumerable<ReadingListResponse>>> GetReadingListByAuthorIdAsync(string authorId);
    }
}

