using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogAPI.Entities.Models
{
	public class ApplicationUser : IdentityUser
	{
        [StringLength(100)]
        [Column(TypeName = "varchar(200)")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        [Column(TypeName = "varchar(200)")]
        public string LastName { get; set; } = string.Empty;

        [StringLength(10)]
        [Column(TypeName = "varchar(50)")]
        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime RegisterDate { get; set; } = DateTime.Now;
    }
}

