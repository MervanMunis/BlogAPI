using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Entities.Models
{
	public class ReadingList
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ReadingListId { get; set; }

        [Required]
        [ForeignKey(nameof(Post))]
        public long PostId { get; set; }
        public Post? Post { get; set; }

        [Required]
        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; } = string.Empty;
        public Author? Author { get; set; }
    }
}

