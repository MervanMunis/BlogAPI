using BlogAPI.Data;
using BlogAPI.DTOs.Response;
using BlogAPI.Entities;
using BlogAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
namespace BlogAPI.Services.Interfaces
{
    public class BookmarkService : IBookmarkService
    {
        private readonly ApplicationDbContext _context;

        public BookmarkService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Method to add a bookmark for a post
        public async Task<ServiceResult<string>> AddBookmarkAsync(long postId, string authorId)
        {
            var post = await _context.Posts!.FindAsync(postId);
            if (post == null)
            {
                return ServiceResult<string>.FailureResult("Post not found");
            }

            // Check if the post is already bookmarked by the author
            if (await _context.Bookmarks!.AnyAsync(bm => bm.PostId == postId && bm.AuthorId == authorId))
            {
                return ServiceResult<string>.FailureResult("Post already bookmarked");
            }

            var bookmark = new Bookmark
            {
                PostId = postId,
                AuthorId = authorId
            };

            _context.Bookmarks!.Add(bookmark);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Post bookmarked successfully");
        }

        // Method to remove a bookmark for a post
        public async Task<ServiceResult<bool>> RemoveBookmarkAsync(long postId, string authorId)
        {
            var bookmark = await _context.Bookmarks!
                .FirstOrDefaultAsync(bm => bm.PostId == postId && bm.AuthorId == authorId);

            if (bookmark == null)
            {
                return ServiceResult<bool>.FailureResult("Bookmark not found");
            }

            _context.Bookmarks!.Remove(bookmark);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true);
        }

        // Method to get all bookmarks by an author
        public async Task<ServiceResult<IEnumerable<BookmarkResponse>>> GetBookmarksByAuthorIdAsync(string authorId)
        {
            var bookmarks = await _context.Bookmarks!
                .Where(bm => bm.AuthorId == authorId)
                .Select(bm => new BookmarkResponse
                {
                    PostId = bm.PostId,
                    PostTitle = bm.Post!.Title,
                    AuthorId = bm.AuthorId
                })
                .ToListAsync();

            return ServiceResult<IEnumerable<BookmarkResponse>>.SuccessResult(bookmarks);
        }
    }
}

