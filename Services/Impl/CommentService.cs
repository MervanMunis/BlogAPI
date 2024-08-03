using BlogAPI.Data;
using BlogAPI.DTOs.Request;
using BlogAPI.DTOs.Response;
using BlogAPI.Entities;
using BlogAPI.Entities.Enums;
using BlogAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
namespace BlogAPI.Services.Interfaces
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDbContext _context;

        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Adds a new comment to a post
        public async Task<ServiceResult<string>> AddCommentAsync(CommentRequest commentRequest)
        {
            // Check if the post exists
            var post = await _context.Posts!.
                FindAsync(commentRequest.PostId);

            if (post == null)
            {
                return ServiceResult<string>.FailureResult("Post not found");
            }

            // Create a new comment
            var comment = new Comment
            {
                Content = commentRequest.Content,
                AuthorId = commentRequest.AuthorId,
                PostId = commentRequest.PostId
            };

            _context.Comments!.Add(comment);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Comment added successfully");
        }

        // Adds a reply to an existing comment
        public async Task<ServiceResult<string>> AddReplyAsync(long parentCommentId, CommentRequest commentRequest)
        {
            // Check if the parent comment exists
            var parentComment = await _context.Comments!.FindAsync(parentCommentId);
            if (parentComment == null)
            {
                return ServiceResult<string>.FailureResult("Parent comment not found");
            }

            // Create a new reply
            var comment = new Comment
            {
                Content = commentRequest.Content,
                AuthorId = commentRequest.AuthorId,
                PostId = parentComment.PostId,
                ParentCommentId = parentCommentId
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Reply added successfully");
        }

        // Retrieves comments by post ID
        public async Task<ServiceResult<IEnumerable<CommentResponse>>> GetCommentsByPostIdAsync(long postId)
        {
            // Fetch the top-level comments for the post
            var comments = await _context.Comments!
                .Include(a => a.Author)
                    .ThenInclude(ap => ap!.ApplicationUser)
                .Include(cl => cl.CommentLikes)
                .Where(c => c.PostId == postId &&
                            c.ParentCommentId == null &&
                            c.Author!.AuthorStatus == Status.Active.ToString())
                .Include(c => c.Replies)!
                .ToListAsync();

            if (comments == null || !comments.Any())
            {
                return ServiceResult<IEnumerable<CommentResponse>>.FailureResult("No comments found");
            }

            var commentResponses = new List<CommentResponse>();

            foreach (var comment in comments)
            {
                var commentResponse = await MapCommentToResponseAsync(comment);
                commentResponses.Add(commentResponse);
            }

            return ServiceResult<IEnumerable<CommentResponse>>.SuccessResult(commentResponses);
        }

        // Retrieves replies for a specific comment
        public async Task<ServiceResult<IEnumerable<CommentResponse>>> GetRepliesByCommentIdAsync(long commentId)
        {
            var replies = await _context.Comments!
                .Include(a => a.Author)
                    .ThenInclude(ap => ap!.ApplicationUser)
                .Include(c => c.CommentLikes)
                .Where(c => c.ParentCommentId == commentId &&
                            c.Author!.AuthorStatus == Status.Active.ToString())
                .Include(c => c.Replies)!
                .ToListAsync();

            var replyResponses = replies.Select(MapCommentToResponse).ToList();

            return ServiceResult<IEnumerable<CommentResponse>>.SuccessResult(replyResponses);
        }

        // Maps a Comment entity to a CommentResponse DTO
        private CommentResponse MapCommentToResponse(Comment comment)
        {
            return new CommentResponse
            {
                CommentId = comment.CommentId,
                Content = comment.IsDeleted ? "This comment has been deleted." : comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                NumberOfLikes = comment.CommentLikes?.Count ?? 0,
                AuthorId = comment.AuthorId,
                AuthorName = comment.Author?.ApplicationUser?.UserName,
                PostId = comment.PostId,
                Replies = comment.Replies?.Select(MapCommentToResponse).ToList() ?? new List<CommentResponse>()
            };
        }

        // Asynchronously maps a Comment entity to a CommentResponse DTO, including nested replies
        private async Task<CommentResponse> MapCommentToResponseAsync(Comment comment)
        {
            var commentResponse = new CommentResponse
            {
                CommentId = comment.CommentId,
                Content = comment.IsDeleted ? "This comment has been deleted." : comment.Content,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt,
                NumberOfLikes = comment.CommentLikes?.Count ?? 0,
                AuthorId = comment.AuthorId,
                AuthorName = comment.Author?.ApplicationUser?.UserName,
                PostId = comment.PostId,
                Replies = new List<CommentResponse>()
            };

            var replies = await _context.Comments!
                .Include(c => c.Author)
                .Include(l => l.CommentLikes)
                .Where(c => c.ParentCommentId == comment.CommentId && c.Author!.AuthorStatus == Status.Active.ToString())
                .ToListAsync();

            if (replies.Any())
            {
                foreach (var reply in replies)
                {
                    var replyResponse = await MapCommentToResponseAsync(reply);
                    commentResponse.Replies.Add(replyResponse);
                }
            }

            return commentResponse;
        }

        // Updates an existing comment
        public async Task<ServiceResult<string>> UpdateCommentAsync(long commentId, CommentRequest commentRequest)
        {
            var comment = await _context.Comments!.FindAsync(commentId);

            if (comment == null)
            {
                return ServiceResult<string>.FailureResult("Comment not found");
            }

            comment.Content = commentRequest.Content;
            comment.UpdatedAt = DateTime.UtcNow;

            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Comment updated successfully");
        }

        // "Deletes" a comment by marking it as deleted instead of physically removing it
        public async Task<ServiceResult<bool>> DeleteCommentAsync(string authorId, long commentId)
        {
            var comment = await _context.Comments!.FindAsync(commentId);

            if (comment == null)
            {
                return ServiceResult<bool>.FailureResult("Comment not found");
            }

            if (comment.Replies != null && comment.Replies.Any())
            {
                comment.IsDeleted = true;
                _context.Comments.Update(comment);
            }
            else
            {
                comment.IsDeleted = true;
                _context.Comments.Update(comment);
            }

            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true);
        }
    }
}

