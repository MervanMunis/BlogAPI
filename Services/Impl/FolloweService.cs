using BlogAPI.Data;
using BlogAPI.Entities.Models;
using BlogAPI.Entities;
using BlogAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using BlogAPI.DTOs.Response;
namespace BlogAPI.Services.Interfaces
{
    public class FollowService : IFollowService
    {
        private readonly ApplicationDbContext _context;

        public FollowService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<string>> FollowAuthorAsync(string authorId, string followedAuthorId)
        {
            if (await _context.Followings!.AnyAsync(f => f.AuthorId == authorId && f.FollowedAuthorId == followedAuthorId))
            {
                return ServiceResult<string>.FailureResult("Already following this author");
            }

            var following = new Following
            {
                AuthorId = authorId,
                FollowedAuthorId = followedAuthorId
            };

            var follower = new Follower
            {
                AuthorId = followedAuthorId,
                FollowedAuthorId = authorId
            };

            _context.Followings!.Add(following);
            _context.Followers!.Add(follower);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Successfully followed the author");
        }

        public async Task<ServiceResult<bool>> UnfollowAuthorAsync(string authorId, string followedAuthorId)
        {
            var following = await _context.Followings!
                .FirstOrDefaultAsync(f => f.AuthorId == authorId && f.FollowedAuthorId == followedAuthorId);

            var follower = await _context.Followers!
                .FirstOrDefaultAsync(f => f.AuthorId == followedAuthorId && f.FollowedAuthorId == authorId);

            if (following == null || follower == null)
            {
                return ServiceResult<bool>.FailureResult("Not following this author");
            }

            _context.Followings!.Remove(following);
            _context.Followers!.Remove(follower);
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true);
        }

        public async Task<ServiceResult<IEnumerable<FollowerResponse>>> GetFollowersByAuthorIdAsync(string authorId)
        {
            var followers = await _context.Followers!
                .Where(f => f.AuthorId == authorId)
                .Select(f => new FollowerResponse
                {
                    FollowerId = f.FollowerId,
                    AuthorId = f.AuthorId,
                    FollowedAuthorId = f.FollowedAuthorId
                })
                .ToListAsync();

            return ServiceResult<IEnumerable<FollowerResponse>>.SuccessResult(followers);
        }

        public async Task<ServiceResult<IEnumerable<FollowingResponse>>> GetFollowingByAuthorIdAsync(string authorId)
        {
            var following = await _context.Followings!
                .Where(f => f.AuthorId == authorId)
                .Select(f => new FollowingResponse
                {
                    FollowingId = f.FollowingId,
                    AuthorId = f.AuthorId,
                    FollowedAuthorId = f.FollowedAuthorId!
                })
                .ToListAsync();

            return ServiceResult<IEnumerable<FollowingResponse>>.SuccessResult(following);
        }
    }
}

