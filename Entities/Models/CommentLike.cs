using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Entities.Models
{
	public class CommentLike
	{
        [Key, Column(Order = 0)]
        public long CommentId { get; set; }

        [ForeignKey(nameof(CommentId))]
        public Comment? Comment { get; set; }

        [Key, Column(Order = 1)]
        public string AuthorId { get; set; } = string.Empty;

        [ForeignKey(nameof(AuthorId))]
        public Author? Author { get; set; }
    }
}

