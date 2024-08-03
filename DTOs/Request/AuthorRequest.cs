using System;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.DTOs.Request
{
	public class AuthorRequest
	{
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 2)]
        public string? DisplayName { get; set; }

        [StringLength(100)]
        public string? UserName { get; set; }

        [StringLength(2000)]
        public string? Bio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [StringLength(10)]
        public string? Gender { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

