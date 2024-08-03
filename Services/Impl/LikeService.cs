using BlogAPI.Data;
using BlogAPI.DTOs.Response;
using BlogAPI.Entities;
using BlogAPI.Entities.Models;
using BlogAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
namespace BlogAPI.Services.Interfaces
{
    public class LikeService : ILikeService
    {
        private readonly ApplicationDbContext _context;

        public LikeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<string>> LikePostAsync(long postId, string authorId)
        {
            var post = await _context.Posts!.FindAsync(postId);

            var author = await _context.Likes!.FirstOrDefaultAsync(a => a.AuthorId == authorId);

            if (post == null)
            {
                return ServiceResult<string>.FailureResult("Post not found");
            }

            if (author != null)
            {
                return ServiceResult<string>.FailureResult("You have already liked the post.");
            }

            var like = new Like
            {
                PostId = postId,
                AuthorId = authorId
            };

            _context.Likes!.Add(like);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Post liked successfully");
        }

        public async Task<ServiceResult<bool>> UnlikePostAsync(long postId, string authorId)
        {
            var like = await _context.Likes!
                .FirstOrDefaultAsync(l => l.PostId == postId && l.AuthorId == authorId);

            if (like == null)
            {
                return ServiceResult<bool>.FailureResult("Like not found");
            }

            _context.Likes!.Remove(like);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true);
        }
    }
}

