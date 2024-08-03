using System;
using BlogAPI.Data;
using BlogAPI.DTOs.Request;
using BlogAPI.DTOs.Response;
using BlogAPI.Entities.Enums;
using BlogAPI.Entities.Models;
using BlogAPI.Exceptions;
using BlogAPI.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Services.Impl
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Method to retrieve all active categories
        public async Task<ServiceResult<IEnumerable<CategoryResponse>>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories!
                .Where(c => c.CategoryStatus == Status.Active.ToString())
                .Include(c => c.SubCategories)
                .Select(c => new CategoryResponse()
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    CategoryStatus = c.CategoryStatus
                })
                .ToListAsync();
            return ServiceResult<IEnumerable<CategoryResponse>>.SuccessResult(categories);
        }

        // Method to retrieve a category by its ID
        public async Task<ServiceResult<CategoryResponse>> GetCategoryByIdAsync(short id)
        {
            var category = await _context.Categories!
                .Include(c => c.SubCategories)
                .Select(c => new CategoryResponse()
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    CategoryStatus = c.CategoryStatus
                })
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            // Check if category exists and is active
            if (category == null || category.CategoryStatus != Status.Active.ToString())
            {
                return ServiceResult<CategoryResponse>.FailureResult("Category not found");
            }

            return ServiceResult<CategoryResponse>.SuccessResult(category);
        }

        // Method to add a new category
        public async Task<ServiceResult<string>> AddCategoryAsync(CategoryRequest categoryRequest)
        {
            // Check if the category name already exists
            if (await _context.Categories!.AnyAsync(c => c.Name == categoryRequest.Name))
            {
                return ServiceResult<string>.FailureResult("The category name already exists!");
            }

            var category = new Category()
            {
                Name = categoryRequest.Name,
            };

            await _context.Categories!.AddAsync(category);
            await _context.SaveChangesAsync();

            return ServiceResult<string>.SuccessMessageResult("Category successfully created.");
        }

        // Method to update an existing category
        public async Task<ServiceResult<bool>> UpdateCategoryAsync(short id, CategoryRequest categoryRequest)
        {
            var existingCategory = await _context.Categories!.FindAsync(id);
            if (existingCategory == null)
            {
                return ServiceResult<bool>.FailureResult("Category not found");
            }

            existingCategory.Name = categoryRequest.Name;
            _context.Update(existingCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return ServiceResult<bool>.SuccessResult(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Categories.AnyAsync(c => c.CategoryId == id))
                {
                    return ServiceResult<bool>.FailureResult("Category not found");
                }
                else
                {
                    throw;
                }
            }
        }

        // Method to set the status of a category and its related subcategories and posts
        public async Task<ServiceResult<bool>> SetCategoryStatusAsync(short id, string status)
        {
            var category = await _context.Categories!.FindAsync(id);

            if (category == null)
            {
                return ServiceResult<bool>.FailureResult("Category not found");
            }

            // Update the category status
            category.CategoryStatus = status.ToString();
            _context.Update(category).State = EntityState.Modified;

            // Retrieve all subcategories of the category
            var subCategories = await _context.SubCategories!
                .Where(sc => sc.CategoryId == id)
                .ToListAsync();

            // Update the status of subcategories and their related posts
            foreach (var subCategory in subCategories)
            {
                subCategory.SubCategoryStatus = status.ToString();
                _context.Entry(subCategory).State = EntityState.Modified;

                if (status == Status.InActive.ToString())
                {
                    var postSubCategories = await _context.PostSubCategory!
                        .Where(psc => psc.SubCategoriesId == subCategory.SubCategoryId)
                        .Include(psc => psc.Post)
                        .ToListAsync();

                    foreach (var postSubCategory in postSubCategories)
                    {
                        postSubCategory.Post!.PostStatus = status.ToString();
                        _context.Entry(postSubCategory.Post).State = EntityState.Modified;
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return ServiceResult<bool>.SuccessResult(true);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Categories.AnyAsync(c => c.CategoryId == id))
                {
                    return ServiceResult<bool>.FailureResult("Category not found");
                }
                else
                {
                    throw;
                }
            }
        }
    }
}

