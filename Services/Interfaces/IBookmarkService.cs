using BlogAPI.DTOs.Response;
using BlogAPI.Exceptions;
namespace BlogAPI.Services.Interfaces
{
    public interface IBookmarkService
	{
        Task<ServiceResult<string>> AddBookmarkAsync(long postId, string authorId);
        Task<ServiceResult<bool>> RemoveBookmarkAsync(long postId, string authorId);
        Task<ServiceResult<IEnumerable<BookmarkResponse>>> GetBookmarksByAuthorIdAsync(string authorId);
    }
}

