using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_WebApi.Models.User
{
    public class UserLoginDto
    {
        [Required]
        [MaxLength(40)]
        public string Email { get; set; } = "";

        [Required]
        [MaxLength(40)]
        public string Password { get; set; } = "";
    }
}
