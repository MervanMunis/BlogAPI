using System;
using BlogAPI.Entities.Enums;
using BlogAPI.Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.DTOs.Request
{
    public class CategoryRequest
	{
        [Required]
        [StringLength(800)]
        public string? Name { get; set; }
    }
}

