using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace BlogAPI.DTOs.Request
{
	public class CommentRequest
	{
        [Required]
        [Column(TypeName = "text")]
        public string Content { get; set; } = string.Empty;

        [Required]
        public long PostId { get; set; }

        [JsonIgnore]
        public string AuthorId { get; set; } = string.Empty;
    }
}

