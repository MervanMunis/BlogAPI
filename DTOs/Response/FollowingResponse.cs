using System;
namespace BlogAPI.DTOs.Response
{
	public class FollowingResponse
	{
        public long FollowingId { get; set; }
        public string AuthorId { get; set; } = string.Empty;
        public string FollowedAuthorId { get; set; } = string.Empty;
    }
}

