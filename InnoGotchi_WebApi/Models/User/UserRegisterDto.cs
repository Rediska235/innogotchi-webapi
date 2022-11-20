using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_WebApi.Models.User
{
    public class UserRegisterDto
    {

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; } = "";


        [Required]
        [MaxLength(20)]
        public string LastName { get; set; } = "";


        [Required]
        [MaxLength(40)]
        public string Email { get; set; } = "";


        [Required]
        [MaxLength(40)]
        public string Password { get; set; } = "";
    }
}
