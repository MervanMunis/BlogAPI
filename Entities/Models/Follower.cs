using System;
using BlogAPI.Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace BlogAPI.Entities
{
	public class Follower
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FollowerId { get; set; }

        [Required]
        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; } = string.Empty;
        public Author? Author { get; set; }

        [Required]
        [ForeignKey(nameof(FollowedAuthor))]
        public string FollowedAuthorId { get; set; } = string.Empty;
        public Author? FollowedAuthor { get; set; }
    }
}

