using BlogAPI.Data;
using BlogAPI.DTOs.Request;
using BlogAPI.DTOs.Response;
using BlogAPI.Entities;
using BlogAPI.Entities.Enums;
using BlogAPI.Entities.Models;
using BlogAPI.Exceptions;
using BlogAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
namespace BlogAPI.Services.Impl
{
    public class PostService : IPostService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileService _fileService;

        public PostService(ApplicationDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<ServiceResult<string>> AddPostAsync(PostRequest postRequest)
        {
            var post = new Post
            {
                Title = postRequest.Title,
                Content = postRequest.Content,
                AuthorId = postRequest.AuthorId,
                CreatedAt = DateTime.UtcNow,
                PostStatus = Status.Active.ToString()
            };

            // Handle Tags
            if (postRequest.Tags != null && postRequest.Tags.Any())
            {
                var existingTags = await _context.Tags!
                    .Where(t => postRequest.Tags.Contains(t.Name!))
                    .ToListAsync();

                var newTags = postRequest.Tags
                    .Where(tagName => existingTags.All(et => et.Name != tagName))
                    .Select(tagName => new Tag { Name = tagName })
                    .ToList();

                if (newTags.Any())
                {
                    await _context.Tags!.AddRangeAsync(newTags);
                    await _context.SaveChangesAsync();
                }

                var allTags = existingTags.Concat(newTags).ToList();
                post.PostTags = allTags.Select(t => new PostTag { TagId = t.TagId }).ToList();
            }

            post = HandleCrossTables(postRequest, post);

            await _context.Posts!.AddAsync(post);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Post successfully created!");
        }

        public async Task<ServiceResult<string>> UpdatePostAsync(long postId, PostRequest postRequest)
        {
            var post = await _context.Posts!
                .Include(p => p.PostTags)!
                .ThenInclude(pt => pt.Tag)
                .FirstOrDefaultAsync(p => p.PostId == postId);
            if (post == null)
            {
                return ServiceResult<string>.FailureResult("Post not found");
            }

            post.Title = postRequest.Title;
            post.Content = postRequest.Content;
            post.UpdatedAt = DateTime.UtcNow;


            // Ensure PostTags is initialized
            post.PostTags ??= new List<PostTag>();

            // Handle Tags
            if (postRequest.Tags != null && postRequest.Tags.Any())
            {
                var existingTags = await _context.Tags!
                    .Where(t => postRequest.Tags.Contains(t.Name!))
                    .ToListAsync();

                var newTags = postRequest.Tags
                    .Where(tagName => existingTags.All(et => et.Name != tagName))
                    .Select(tagName => new Tag { Name = tagName })
                    .ToList();

                if (newTags.Any())
                {
                    await _context.Tags!.AddRangeAsync(newTags);
                    await _context.SaveChangesAsync();
                }

                var allTags = existingTags.Concat(newTags).ToList();

                // Remove old tags that are not in the new list
                var tagsToRemove = post.PostTags!.Where(pt => !postRequest.Tags.Contains(pt.Tag!.Name!)).ToList();
                foreach (var tagToRemove in tagsToRemove)
                {
                    post.PostTags!.Remove(tagToRemove);
                }

                // Add new tags
                foreach (var tag in allTags)
                {
                    if (!post.PostTags!.Any(pt => pt.TagId == tag.TagId))
                    {
                        post.PostTags!.Add(new PostTag { TagId = tag.TagId });
                    }
                }
            }

            post = HandleCrossTables(postRequest, post);

            _context.Posts!.Update(post);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Post successfully updated!");
        }

        public async Task<ServiceResult<PostResponse>> GetPostByIdAsync(long postId)
        {
            var postResponse = await _context.Posts!
                .Where(p => p.PostStatus == Status.Active.ToString())
                .Include(p => p.Author)
                .Include(l => l.Likes)
                .Include(p => p.PostTags)!
                    .ThenInclude(pt => pt.Tag)
                .Include(p => p.PostSubCategories)!
                    .ThenInclude(psc => psc.SubCategory)
                .Select(post => new PostResponse
                {
                    PostId = post.PostId,
                    Title = post.Title,
                    Content = post.Content,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt,
                    NumberOfLikes = post.Likes!.Count,
                    PostStatus = post.PostStatus,
                    AuthorId = post.AuthorId,
                    AuthorName = post.Author!.DisplayName,
                    Tags = post.PostTags!.Select(pt => new TagResponse { TagId = pt.TagId, Name = pt.Tag!.Name }).ToList(),
                    SubCategories = post.PostSubCategories!.Select(psc => new SubCategoryResponse { SubCategoryId = psc.SubCategoriesId, Name = psc.SubCategory!.Name }).ToList()
                })
                .FirstOrDefaultAsync(p => p.PostId == postId);

            if (postResponse == null)
            {
                return ServiceResult<PostResponse>.FailureResult("Post not found");
            }

            return ServiceResult<PostResponse>.SuccessResult(postResponse);
        }

        public async Task<ServiceResult<IEnumerable<PostResponse>>> GetActivePostsAsync()
        {
            var posts = await _context.Posts!
                .Where(p => p.PostStatus == Status.Active.ToString())
                .Include(p => p.Author)
                .Include(l => l.Likes)
                .Include(p => p.PostTags)!
                    .ThenInclude(pt => pt.Tag)
                .Include(p => p.PostSubCategories)!
                    .ThenInclude(psc => psc.SubCategory).ThenInclude(a => a!.Category)
                .Select(p => new PostResponse
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    NumberOfLikes = p.Likes!.Count,
                    PostStatus = p.PostStatus,
                    AuthorId = p.AuthorId,
                    AuthorName = p.Author!.ApplicationUser!.Name,
                    Tags = p.PostTags!.Select(pt => new TagResponse { TagId = pt.TagId, Name = pt.Tag!.Name }).ToList(),
                    SubCategories = p.PostSubCategories!.Select(psc => new SubCategoryResponse {
                        SubCategoryId = psc.SubCategoriesId,
                        Name = psc.SubCategory!.Name,
                        CategoryId = psc.SubCategory.CategoryId,
                        SubCategoryStatus = psc.SubCategory.SubCategoryStatus }).ToList(),
                    Categories = p.PostSubCategories!.Select(c => new CategoryResponse {
                        CategoryId = c.SubCategory!.CategoryId,
                        Name = c.SubCategory.Category!.Name,
                        CategoryStatus = c.SubCategory.Category.CategoryStatus
                    }).ToList()
                })
                .ToListAsync();

            return ServiceResult<IEnumerable<PostResponse>>.SuccessResult(posts);
        }

        public async Task<ServiceResult<IEnumerable<PostResponse>>> GetAllPostsAsync()
        {
            var posts = await _context.Posts!
                .Include(p => p.Author)
                .Include(l => l.Likes)
                .Include(p => p.PostTags)!
                .ThenInclude(pt => pt.Tag)
                .Include(p => p.PostSubCategories)!
                .ThenInclude(ps => ps.SubCategory)
                .ThenInclude(a => a!.Category)
                .Select(p => new PostResponse
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    NumberOfLikes = p.Likes!.Count,
                    PostStatus = p.PostStatus,
                    AuthorId = p.AuthorId,
                    AuthorName = p.Author!.ApplicationUser!.Name,
                    Tags = p.PostTags!.Select(pt => new TagResponse { TagId = pt.TagId, Name = pt.Tag!.Name }).ToList(),
                    SubCategories = p.PostSubCategories!.Select(psc => new SubCategoryResponse
                    {
                        SubCategoryId = psc.SubCategoriesId,
                        Name = psc.SubCategory!.Name,
                        CategoryId = psc.SubCategory.CategoryId,
                        SubCategoryStatus = psc.SubCategory.SubCategoryStatus
                    }).ToList(),
                    Categories = p.PostSubCategories!.Select(c => new CategoryResponse
                    {
                        CategoryId = c.SubCategory!.CategoryId,
                        Name = c.SubCategory.Category!.Name,
                        CategoryStatus = c.SubCategory.Category.CategoryStatus
                    }).ToList()
                })
                .ToListAsync();

            return ServiceResult<IEnumerable<PostResponse>>.SuccessResult(posts);
        }

        public async Task<ServiceResult<IEnumerable<PostResponse>>> GetBannedPostsAsync()
        {
            var posts = await _context.Posts!
                .Where(p => p.PostStatus == Status.Banned.ToString())
                .Include(p => p.Author)
                .Include(l => l.Likes)
                .Include(p => p.PostTags)!
                .ThenInclude(pt => pt.Tag)
                .Include(p => p.PostSubCategories)!
                .ThenInclude(ps => ps.SubCategory)
                .ThenInclude(a => a!.Category)
                .Select(p => new PostResponse
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    NumberOfLikes = p.Likes!.Count,
                    PostStatus = p.PostStatus,
                    AuthorId = p.AuthorId,
                    AuthorName = p.Author!.ApplicationUser!.Name,
                    Tags = p.PostTags!.Select(pt => new TagResponse { TagId = pt.TagId, Name = pt.Tag!.Name }).ToList(),
                    SubCategories = p.PostSubCategories!.Select(psc => new SubCategoryResponse
                    {
                        SubCategoryId = psc.SubCategoriesId,
                        Name = psc.SubCategory!.Name,
                        CategoryId = psc.SubCategory.CategoryId,
                        SubCategoryStatus = psc.SubCategory.SubCategoryStatus
                    }).ToList(),
                    Categories = p.PostSubCategories!.Select(c => new CategoryResponse
                    {
                        CategoryId = c.SubCategory!.CategoryId,
                        Name = c.SubCategory.Category!.Name,
                        CategoryStatus = c.SubCategory.Category.CategoryStatus
                    }).ToList()
                })
                .ToListAsync();

            return ServiceResult<IEnumerable<PostResponse>>.SuccessResult(posts);
        }

        public async Task<ServiceResult<IEnumerable<PostResponse>>> GetDeactivatedPostsAsync()
        {
            var posts = await _context.Posts!
                .Where(p => p.PostStatus == Status.InActive.ToString())
                .Include(p => p.Author)
                .Include(l => l.Likes)
                .Include(p => p.PostTags)!
                .ThenInclude(pt => pt.Tag)
                .Include(p => p.PostSubCategories)!
                .ThenInclude(ps => ps.SubCategory)
                .ThenInclude(a => a!.Category)
                .Select(p => new PostResponse
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    NumberOfLikes = p.Likes!.Count,
                    PostStatus = p.PostStatus,
                    AuthorId = p.AuthorId,
                    AuthorName = p.Author!.ApplicationUser!.Name,
                    Tags = p.PostTags!.Select(pt => new TagResponse { TagId = pt.TagId, Name = pt.Tag!.Name }).ToList(),
                    SubCategories = p.PostSubCategories!.Select(psc => new SubCategoryResponse
                    {
                        SubCategoryId = psc.SubCategoriesId,
                        Name = psc.SubCategory!.Name,
                        CategoryId = psc.SubCategory.CategoryId,
                        SubCategoryStatus = psc.SubCategory.SubCategoryStatus
                    }).ToList(),
                    Categories = p.PostSubCategories!.Select(c => new CategoryResponse
                    {
                        CategoryId = c.SubCategory!.CategoryId,
                        Name = c.SubCategory.Category!.Name,
                        CategoryStatus = c.SubCategory.Category.CategoryStatus
                    }).ToList()
                })
                .ToListAsync();

            return ServiceResult<IEnumerable<PostResponse>>.SuccessResult(posts);
        }

        public async Task<ServiceResult<IEnumerable<TagedPostResponse>>> GetPostsByTagAsync(string tagName)
        {
            var tag = await _context.Tags!.FirstOrDefaultAsync(t => t.Name == tagName);
            if (tag == null)
            {
                return ServiceResult<IEnumerable<TagedPostResponse>>.FailureResult("Tag not found");
            }

            var posts = await _context.PostTag!
                .Where(pt => pt.TagId == tag.TagId)
                .Select(pt => pt.Post)
                .Where(p => p!.PostStatus == Status.Active.ToString())
                .Select(p => new TagedPostResponse
                {
                    PostId = p!.PostId,
                    Title = p.Title,
                    CreatedAt = p.CreatedAt,
                    AuthorName = p.Author!.ApplicationUser!.Name,
                    SubCategories = p.PostSubCategories!.Select(psc => new SubCategoryResponse { SubCategoryId = psc.SubCategoriesId, Name = psc.SubCategory!.Name }).ToList()
                })
                .ToListAsync();

            return ServiceResult<IEnumerable<TagedPostResponse>>.SuccessResult(posts);
        }

        public async Task<ServiceResult<bool>> RemoveImageFromPostAsync(long postId)
        {
            var post = await _context.Posts!.FindAsync(postId);
            if (post == null)
            {
                return ServiceResult<bool>.FailureResult("Post not found");
            }

            await _fileService.DeleteFileAsync(post.ImageUrl!);
            post.ImageUrl = null;

            _context.Update(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.SuccessResult(true);
        }

        public async Task<ServiceResult<string>> BanPostAsync(long postId)
        {
            var post = await _context.Posts!.FindAsync(postId);
            if (post == null)
            {
                return ServiceResult<string>.FailureResult("Post not found");
            }

            post.PostStatus = Status.Banned.ToString();
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Post successfully banned!");
        }

        public async Task<ServiceResult<string>> DeactivatePostAsync(long postId)
        {
            var post = await _context.Posts!.FindAsync(postId);
            if (post == null)
            {
                return ServiceResult<string>.FailureResult("Post not found");
            }

            post.PostStatus = Status.InActive.ToString();
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Post successfully deactivated!");
        }

        public async Task<ServiceResult<string>> ActivatePostAsync(long postId)
        {
            var post = await _context.Posts!.FindAsync(postId);
            if (post == null)
            {
                return ServiceResult<string>.FailureResult("Post not found");
            }

            post.PostStatus = Status.Active.ToString();
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Post successfully activated!");
        }

        public async Task<ServiceResult<string>> AddImageToPostAsync(long postId, IFormFile imageRequest)
        {
            var post = await _context.Posts!.FindAsync(postId);
            if (post == null)
            {
                return ServiceResult<string>.FailureResult("Post not found");
            }

            var imageUrl = await _fileService.SaveFileAsync(imageRequest, "Düzelt");
            post.ImageUrl = imageUrl;

            _context.Update(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Image added to post successfully");
        }

        private Post HandleCrossTables(PostRequest postRequest, Post post)
        {

            // Handle PostSubCategories
            if (postRequest.SubCategoryIds != null && postRequest.SubCategoryIds.Any())
            {
                post.PostSubCategories = postRequest.SubCategoryIds.Select(subCategoryId => new PostSubCategory
                {
                    SubCategoriesId = subCategoryId,
                    PostId = post.PostId
                }).ToList();
            }

            return post;
        }
    }
}

