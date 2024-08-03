using System;
using BlogAPI.Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BlogAPI.Entities.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Entities.Models
{
	public class Category
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short CategoryId { get; set; }

        [Required]
        [StringLength(800)]
        [Column(TypeName = "varchar(800)")]
        public string? Name { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string CategoryStatus { get; set; } = Status.Active.ToString();

        [JsonIgnore]
        public ICollection<Post>? Posts { get; set; }

        [JsonIgnore]
        public ICollection<SubCategory>? SubCategories { get; set; }
    }
}

