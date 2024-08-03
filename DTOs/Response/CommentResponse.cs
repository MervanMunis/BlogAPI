using System;
namespace BlogAPI.DTOs.Response
{
	public class CommentResponse
	{
        public long CommentId { get; set; }

        public string Content { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public int? NumberOfLikes { get; set; }

        public string AuthorId { get; set; } = string.Empty;

        public string? AuthorName { get; set; }

        public long PostId { get; set; }

        public List<CommentResponse> Replies { get; set; } = new List<CommentResponse>();
    }
}

