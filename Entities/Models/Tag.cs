using System;
using BlogAPI.Entities.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.Entities
{
	public class Tag
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TagId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string? Name { get; set; }

        public ICollection<PostTag>? PostTags { get; set; }
    }
}

