using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BlogAPI.Entities.Enums;

namespace BlogAPI.Entities.Models
{
    public class Author
	{
        public string? AuthorId { get; set; } = string.Empty;

        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser? ApplicationUser { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        [Column(TypeName = "nvarchar(100)")]
        public string DisplayName { get; set; } = string.Empty;

        [StringLength(2000)]
        [Column(TypeName = "nvarchar(2000)")]
        public string? Bio { get; set; }

        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string? ProfilePicture { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string AuthorStatus { get; set; } = Status.Active.ToString();


        //Navigation Properties:

        [JsonIgnore]
        public virtual ICollection<Post>? Posts { get; set; }

        [JsonIgnore]
        public virtual ICollection<Comment>? Comments { get; set; }

        [JsonIgnore]
        public virtual ICollection<Like>? Likes { get; set; }

        [JsonIgnore]
        public virtual ICollection<Bookmark>? Bookmarks { get; set; }

        [JsonIgnore]
        public virtual ICollection<Follower>? Followers { get; set; }

        [JsonIgnore]
        public virtual ICollection<Follower>? Following { get; set; }

        [JsonIgnore]
        public virtual ICollection<ReadingList>? ReadingLists { get; set; }

    }
}

