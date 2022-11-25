using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_WebApi.Models.UserModels
{
    public class ChangeUsernameDto
    {
        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; } = "";

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; } = "";
    }
}
