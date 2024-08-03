using BlogAPI.DTOs.Response;
using BlogAPI.Exceptions;

namespace BlogAPI.Services.Interfaces
{
    public interface ICommentLikeService
    {
        Task<ServiceResult<string>> LikeCommentAsync(long commentId, string authorId);
        Task<ServiceResult<bool>> UnlikeCommentAsync(long commentId, string authorId);
        Task<ServiceResult<IEnumerable<CommentLikeResponse>>> GetLikesByCommentIdAsync(long commentId);
    }
}
