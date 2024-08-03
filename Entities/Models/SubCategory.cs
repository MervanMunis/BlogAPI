using System;
using BlogAPI.Entities.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlogAPI.Entities.Models
{
	public class SubCategory
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public short SubCategoryId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string SubCategoryStatus { get; set; } = Status.Active.ToString();

        public short? CategoryId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

        [JsonIgnore]
        public ICollection<PostSubCategory>? PostSubCategories { get; set; }
    }
}

