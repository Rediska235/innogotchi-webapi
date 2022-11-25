using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_WebApi.Models.UserModels
{
    public class UserLoginDto
    {
        [Required]
        [MaxLength(40)]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [MaxLength(40)]
        public string Password { get; set; } = "";
    }
}
