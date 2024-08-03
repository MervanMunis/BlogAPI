using BlogAPI.DTOs.Request;
using BlogAPI.DTOs.Response;
using BlogAPI.Exceptions;
using System;
namespace BlogAPI.Services.Interfaces
{
	public interface ICommentService
	{
        Task<ServiceResult<string>> AddCommentAsync(CommentRequest commentRequest);
        Task<ServiceResult<string>> AddReplyAsync(long parentCommentId, CommentRequest commentRequest);
        Task<ServiceResult<IEnumerable<CommentResponse>>> GetCommentsByPostIdAsync(long postId);
        Task<ServiceResult<IEnumerable<CommentResponse>>> GetRepliesByCommentIdAsync(long commentId);
        Task<ServiceResult<string>> UpdateCommentAsync(long commentId, CommentRequest commentRequest);
        Task<ServiceResult<bool>> DeleteCommentAsync(string authorId, long commentId);
    }
}

