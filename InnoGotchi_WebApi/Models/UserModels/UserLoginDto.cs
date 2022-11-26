﻿using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_WebApi.Models.UserModels
{
    public class UserLoginDto
    {
        [Required]
        [MaxLength(40)]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(40, MinimumLength = 8)]
        public string Password { get; set; } = "";
    }
}
