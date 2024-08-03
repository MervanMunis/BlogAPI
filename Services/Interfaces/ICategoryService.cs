using System;
using BlogAPI.DTOs.Request;
using BlogAPI.DTOs.Response;
using BlogAPI.Entities.Enums;
using BlogAPI.Exceptions;

namespace BlogAPI.Services.Interfaces
{
	public interface ICategoryService
	{
        Task<ServiceResult<IEnumerable<CategoryResponse>>> GetAllCategoriesAsync();
        Task<ServiceResult<CategoryResponse>> GetCategoryByIdAsync(short id);
        Task<ServiceResult<string>> AddCategoryAsync(CategoryRequest categoryRequest);
        Task<ServiceResult<bool>> UpdateCategoryAsync(short id, CategoryRequest categoryRequest);
        Task<ServiceResult<bool>> SetCategoryStatusAsync(short id, string status);
    }
}

