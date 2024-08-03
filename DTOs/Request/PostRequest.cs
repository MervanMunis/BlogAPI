using BlogAPI.DTOs.Response;
using NuGet.Protocol.Plugins;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BlogAPI.DTOs.Request
{
	public class PostRequest
	{
        [Required]
        [StringLength(300, MinimumLength = 5)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        [JsonIgnore]
        public string AuthorId { get; set; } = string.Empty;

        public List<string>? Tags { get; set; }
        public List<short>? SubCategoryIds { get; set; }
    }
}

