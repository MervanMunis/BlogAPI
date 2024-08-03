using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BlogAPI.Entities.Models;

namespace BlogAPI.Entities
{
    public class Comment
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CommentId { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string Content { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;

        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; } = string.Empty;

        [JsonIgnore]
        public Author? Author { get; set; }
 
        [ForeignKey(nameof(Post))]
        public long PostId { get; set; }

        [JsonIgnore]
        public Post? Post { get; set; }

        [ForeignKey(nameof(ParentComment))]
        public long? ParentCommentId { get; set; }

        [JsonIgnore]
        public Comment? ParentComment { get; set; }

        [JsonIgnore]
        public ICollection<Comment>? Replies { get; set; }

        [JsonIgnore]
        public ICollection<CommentLike>? CommentLikes { get; set; }
    }
}

