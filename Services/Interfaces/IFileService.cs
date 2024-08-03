using System;
namespace BlogAPI.Services.Interfaces
{
	public interface IFileService
	{
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        Task<bool> DeleteFileAsync(string filePath);
        Task<byte[]> GetImageByAuthorIdAsync(string authorId);
        Task<byte[]> GetImageByBookIdAsync(long bookId);
    }
}

