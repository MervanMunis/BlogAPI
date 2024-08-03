using System;
namespace BlogAPI.DTOs.Response
{
	public class FollowerResponse
	{
        public long FollowerId { get; set; }
        public string AuthorId { get; set; } = string.Empty;
        public string FollowedAuthorId { get; set; } = string.Empty;
    }
}

