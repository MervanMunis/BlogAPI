using BlogAPI.Entities.Enums;
using BlogAPI.Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.DTOs.Response
{
    public class PostResponse
    {
        public long PostId { get; set; }

        public string? Title { get; set; }

        public string? Content { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int NumberOfLikes { get; set; }

        public string? PostStatus { get; set; }

        public string? AuthorId { get; set; }

        public string? AuthorName { get; set; }

        public List<TagResponse>? Tags { get; set; }

        public List<SubCategoryResponse>? SubCategories { get; set; }

        public List<CategoryResponse>? Categories { get; set; }

    }
}
