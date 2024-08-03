using System;
using BlogAPI.Entities.Enums;
using BlogAPI.Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.DTOs.Request
{
	public class SubCategoryRequest
	{
        [Required]
        [StringLength(100)]
        public string? Name { get; set; }

        public short? CategoryId { get; set; }
    }
}

