using System;
using BlogAPI.Entities.Enums;
using BlogAPI.Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPI.DTOs.Response
{
	public class SubCategoryResponse
	{
        public short? SubCategoryId { get; set; }

        public string? Name { get; set; } 

        public string? SubCategoryStatus { get; set; }

        public short? CategoryId { get; set; }
    }
}

