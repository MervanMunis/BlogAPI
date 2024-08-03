using BlogAPI.Data;
using BlogAPI.DTOs.Request;
using BlogAPI.DTOs.Response;
using BlogAPI.Entities.Enums;
using BlogAPI.Entities.Models;
using BlogAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
namespace BlogAPI.Services.Interfaces
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ApplicationDbContext _context;

        public SubCategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<IEnumerable<SubCategoryResponse>>> GetAllSubCategoriesAsync()
        {
            var subCategories = await _context.SubCategories!
                .Include(c => c.Category)
                .Where(sc => sc.SubCategoryStatus == Status.Active.ToString())
                .Select(sb => new SubCategoryResponse()
                {
                    SubCategoryId = sb.SubCategoryId,
                    Name = sb.Name,
                    SubCategoryStatus = sb.SubCategoryStatus,
                    CategoryId = sb.CategoryId
                })
                .ToListAsync();

            return ServiceResult<IEnumerable<SubCategoryResponse>>.SuccessResult(subCategories);
        }

        public async Task<ServiceResult<SubCategoryResponse>> GetSubCategoryByIdAsync(short id)
        {
            var subCategory = await _context.SubCategories!
                .Where(sc => sc.SubCategoryStatus == Status.Active.ToString())
                .Select(sb => new SubCategoryResponse()
                {
                    SubCategoryId = sb.SubCategoryId,
                    Name = sb.Name,
                    SubCategoryStatus = sb.SubCategoryStatus,
                })
                .FirstOrDefaultAsync(sc => sc.SubCategoryId == id);

            if (subCategory == null || subCategory.SubCategoryStatus != Status.Active.ToString())
            {
                return ServiceResult<SubCategoryResponse>.FailureResult("SubCategory not found");
            }

            return ServiceResult<SubCategoryResponse>.SuccessResult(subCategory);
        }

        public async Task<ServiceResult<string>> AddSubCategoryAsync(SubCategoryRequest subCategoryRequest)
        {
            if (await _context.SubCategories!.AnyAsync(sc => sc.Name == subCategoryRequest.Name))
            {
                return ServiceResult<string>.FailureResult("The subcategory name already exists!");
            }

            var subCategory = new SubCategory()
            {
                Name = subCategoryRequest.Name!,
                CategoryId = subCategoryRequest.CategoryId
            };

            await _context.SubCategories!.AddAsync(subCategory);
            await _context.SaveChangesAsync();
            return ServiceResult<string>.SuccessMessageResult("SubCategory successfully created!");
        }

        public async Task<ServiceResult<bool>> UpdateSubCategoryAsync(short id, SubCategoryRequest subCategoryRequest)
        {
            var existingSubCategory = await _context.SubCategories!.FindAsync(id);
            if (existingSubCategory == null)
            {
                return ServiceResult<bool>.FailureResult("SubCategory not found");
            }

            existingSubCategory.Name = subCategoryRequest.Name!;
            existingSubCategory.CategoryId = subCategoryRequest.CategoryId;
            _context.Update(existingSubCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return ServiceResult<bool>.SuccessResult(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.SubCategories.AnyAsync(sc => sc.SubCategoryId == id))
                {
                    return ServiceResult<bool>.FailureResult("SubCategory not found");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<ServiceResult<bool>> SetSubCategoryStatusAsync(short id, string status)
        {
            var subCategory = await _context.SubCategories!.FindAsync(id);
            if (subCategory == null)
            {
                return ServiceResult<bool>.FailureResult("SubCategory not found");
            }

            subCategory.SubCategoryStatus = status;
            _context.Update(subCategory).State = EntityState.Modified;

            if (status == Status.InActive.ToString())
            {
                var postSubCategories = await _context.PostSubCategory!
                    .Where(psc => psc.SubCategoriesId == id)
                    .Include(psc => psc.Post)
                    .ToListAsync();

                foreach (var postSubCategory in postSubCategories)
                {
                    postSubCategory.Post!.PostStatus = status;
                    _context.Update(postSubCategory.Post).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return ServiceResult<bool>.SuccessResult(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.SubCategories.AnyAsync(e => e.SubCategoryId == id))
                {
                    return ServiceResult<bool>.FailureResult("SubCategory not found");
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<ServiceResult<IEnumerable<PostResponse>>> GetPostsBySubCategoryIdAsync(short subCategoryId)
        {
            var posts = await _context.Posts!
                .Include(p => p.PostSubCategories)!
                    .ThenInclude(psc => psc.SubCategory)
                .Where(p => p.PostSubCategories!.Any(psc => psc.SubCategoriesId == subCategoryId))
                .Select(p => new PostResponse()
                {
                    PostId = p.PostId,
                    Title = p.Title,
                    Content = p.Content
                   // SubCategoryName = p.PostSubCategories!.Select(psc => psc.SubCategory!.Name).First()
                })
                .ToListAsync();

            if (!posts.Any())
            {
                return ServiceResult<IEnumerable<PostResponse>>.FailureResult("No posts found for this subcategory");
            }

            return ServiceResult<IEnumerable<PostResponse>>.SuccessResult(posts);
        }
    }
}

