using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.DTOs.Request
{
	public class TagRequest
	{
        [Required]
        [StringLength(100)]
        [Column(TypeName = "varchar(100)")]
        public string? Name { get; set; }
    }
}

