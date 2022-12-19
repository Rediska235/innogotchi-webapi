﻿using System.ComponentModel.DataAnnotations;

namespace InnoGotchi.Application.Dtos.UserModels
{
    public class UserRegisterDto
    {
        [Required]
        [MaxLength(20)]
        [StringLength(20, MinimumLength = 2)]
        public string FirstName { get; set; } = "";
        
        [Required]
        [MaxLength(20)]
        [StringLength(20, MinimumLength = 2)]
        public string LastName { get; set; } = "";
        
        [Required]
        [MaxLength(40)]
        [EmailAddress]
        public string Email { get; set; } = "";
        
        [Required]
        [MaxLength(40)]
        [StringLength(40, MinimumLength = 8)]
        public string Password { get; set; } = "";
    }
}
