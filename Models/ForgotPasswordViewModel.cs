﻿using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
