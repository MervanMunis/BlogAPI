using BlogAPI.DTOs.Request;
using BlogAPI.DTOs.Response;
using BlogAPI.Exceptions;
using System;
namespace BlogAPI.Services.Interfaces
{
	public interface ISubCategoryService
	{
        Task<ServiceResult<IEnumerable<SubCategoryResponse>>> GetAllSubCategoriesAsync();
        Task<ServiceResult<SubCategoryResponse>> GetSubCategoryByIdAsync(short id);
        Task<ServiceResult<string>> AddSubCategoryAsync(SubCategoryRequest subCategoryRequest);
        Task<ServiceResult<bool>> UpdateSubCategoryAsync(short id, SubCategoryRequest subCategoryRequest);
        Task<ServiceResult<bool>> SetSubCategoryStatusAsync(short id, string status);
        Task<ServiceResult<IEnumerable<PostResponse>>> GetPostsBySubCategoryIdAsync(short subCategoryId);
    }
}

