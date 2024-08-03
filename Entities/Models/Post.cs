using BlogAPI.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogAPI.Entities.Models
{
    public class Post
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PostId { get; set; } 

        [Required]
        [StringLength(300, MinimumLength = 5, ErrorMessage = "Title's length must be between 5 and 300.")]
        [Column(TypeName = "nvarchar(300)")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "text")]
        public string Content { get; set; } = string.Empty;

        [Column(TypeName = "nvarchar(200)")]
        public string? ImageUrl { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string PostStatus { get; set; } = Status.Active.ToString();


        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; } = string.Empty;

        [JsonIgnore]
        public Author? Author { get; set; }


        [JsonIgnore]
        public ICollection<Comment>? Comments { get; set; }

        [JsonIgnore]
        public ICollection<Like>? Likes { get; set; }

        [JsonIgnore]
        public ICollection<Bookmark>? Bookmarks { get; set; }

        [JsonIgnore]
        public ICollection<PostTag>? PostTags { get; set; }

        [JsonIgnore]
        public ICollection<PostSubCategory>? PostSubCategories { get; set; }

        [JsonIgnore]
        public ICollection<ReadingList>? ReadingLists { get; set; }
    }
}

