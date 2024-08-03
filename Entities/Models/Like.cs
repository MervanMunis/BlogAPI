using System;
using BlogAPI.Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Entities
{
	public class Like
	{
        [Key, Column(Order = 0)]
        public long PostId { get; set; }

        [ForeignKey("PostId")]
        public Post? Post { get; set; }
        
        [Key, Column(Order = 1)]
        public string AuthorId { get; set; } = string.Empty;

        [ForeignKey(nameof(AuthorId))]
        public Author? Author { get; set; }
    }
}

