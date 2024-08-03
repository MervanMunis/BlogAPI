using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Entities.Models
{
	public class Following
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FollowingId { get; set; }

        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; } = string.Empty;
        public Author? Author { get; set; }

        [ForeignKey(nameof(FollowedAuthor))]
        public string? FollowedAuthorId { get; set; }
        public Author? FollowedAuthor { get; set; }
    }
}

