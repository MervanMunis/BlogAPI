using BlogAPI.Data;
using BlogAPI.DTOs.Response;
using BlogAPI.Entities.Models;
using BlogAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
namespace BlogAPI.Services.Interfaces
{
    public class ReadingListService : IReadingListService
    {
        private readonly ApplicationDbContext _context;

        public ReadingListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<string>> AddToReadingListAsync(long postId, string authorId)
        {
            var post = await _context.Posts!.FindAsync(postId);
            if (post == null)
            {
                return ServiceResult<string>.FailureResult("Post not found");
            }

            if (await _context.ReadingLists!.AnyAsync(rl => rl.PostId == postId && rl.AuthorId == authorId))
            {
                return ServiceResult<string>.FailureResult("Post already in reading list");
            }

            var readingList = new ReadingList
            {
                PostId = postId,
                AuthorId = authorId
            };

            _context.ReadingLists!.Add(readingList);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Post added to reading list successfully");
        }

        public async Task<ServiceResult<bool>> RemoveFromReadingListAsync(long postId, string authorId)
        {
            var readingList = await _context.ReadingLists!
                .FirstOrDefaultAsync(rl => rl.PostId == postId && rl.AuthorId == authorId);

            if (readingList == null)
            {
                return ServiceResult<bool>.FailureResult("Reading list entry not found");
            }

            _context.ReadingLists!.Remove(readingList);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true);
        }

        public async Task<ServiceResult<IEnumerable<ReadingListResponse>>> GetReadingListByAuthorIdAsync(string authorId)
        {
            var readingList = await _context.ReadingLists!
                .Where(rl => rl.AuthorId == authorId)
                .Select(rl => new ReadingListResponse
                {
                    ReadingListId = rl.ReadingListId,
                    PostId = rl.PostId,
                    PostTitle = rl.Post!.Title,
                    AuthorId = rl.AuthorId
                })
                .ToListAsync();

            return ServiceResult<IEnumerable<ReadingListResponse>>.SuccessResult(readingList);
        }
    }
}

