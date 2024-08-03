using System;
using BlogAPI.Entities.Enums;

namespace BlogAPI.DTOs.Response
{
	public class AuthorResponse
	{
        public string? AuthorId { get; set; }

        public string? Name { get; set; }

        public string? LastName { get; set; }

        public string? UserName { get; set; }

        public string? DisplayName { get; set; } 

        public string? Bio { get; set; }

        public string? Email { get; set; } 

        public string? Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTime? RegisterDate { get; set; }

        public string? AuthorStatus { get; set; }
    }
}

