using System;
namespace BlogAPI.DTOs.Response
{
	public class BookmarkResponse
	{
        public long PostId { get; set; }
        public string AuthorId { get; set; } = string.Empty;
        public string PostTitle { get; set; } = string.Empty;
    }
}

