using System.ComponentModel.DataAnnotations;

namespace BlogAPI.DTOs.Request
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}
