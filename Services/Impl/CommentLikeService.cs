using BlogAPI.Data;
using BlogAPI.DTOs.Response;
using BlogAPI.Entities.Models;
using BlogAPI.Exceptions;
using BlogAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Services.Impl
{
    public class CommentLikeService : ICommentLikeService
    {
        private readonly ApplicationDbContext _context;

        public CommentLikeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<string>> LikeCommentAsync(long commentId, string authorId)
        {
            var comment = await _context.Comments!.FindAsync(commentId);

            var author = await _context.CommentLikes!.FirstOrDefaultAsync(a => a.AuthorId == authorId && a.CommentId == commentId);

            if (author != null)
            {
                return ServiceResult<string>.FailureResult("You have already liked the comment.");
            }

            if (comment == null)
            {
                return ServiceResult<string>.FailureResult("Comment not found");
            }

            var commentLike = new CommentLike
            {
                CommentId = commentId,
                AuthorId = authorId
            };

            _context.CommentLikes!.Add(commentLike);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Comment liked successfully");
        }

        public async Task<ServiceResult<bool>> UnlikeCommentAsync(long commentId, string authorId)
        {
            var commentLike = await _context.CommentLikes!
                .FirstOrDefaultAsync(cl => cl.CommentId == commentId && cl.AuthorId == authorId);

            if (commentLike == null)
            {
                return ServiceResult<bool>.FailureResult("Like not found");
            }

            _context.CommentLikes!.Remove(commentLike);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true);
        }

        public async Task<ServiceResult<IEnumerable<CommentLikeResponse>>> GetLikesByCommentIdAsync(long commentId)
        {
            var likes = await _context.CommentLikes!
                .Include(a => a.Author)
                .Where(cl => cl.CommentId == commentId)
                .Select(cl => new CommentLikeResponse
                {
                    AuthorUsername = cl.Author!.ApplicationUser!.UserName,
                    CommentId = cl.CommentId,
                    AuthorId = cl.AuthorId
                })
                .ToListAsync();

            return ServiceResult<IEnumerable<CommentLikeResponse>>.SuccessResult(likes);
        }
    }
}
