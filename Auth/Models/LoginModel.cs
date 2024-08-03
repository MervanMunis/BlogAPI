﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Auth.Models
{
	public class LoginModel
	{
        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}

