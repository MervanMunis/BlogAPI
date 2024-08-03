using System;
namespace BlogAPI.DTOs.Response
{
	public class CommentLikeResponse
	{
        public long CommentId { get; set; }

        public string? AuthorId { get; set; }

        public string AuthorUsername { get; set; } = string.Empty;
    }
}

