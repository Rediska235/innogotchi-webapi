using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_WebApi.Models.UserModels
{
    public class ChangePasswordDto
    {
        [Required]
        [MaxLength(40)]
        public string OldPassword { get; set; } = "";

        [Required]
        [MaxLength(40)]
        public string NewPassword { get; set; } = "";

        [Required]
        [MaxLength(40)]
        public string ConfirmNewPassword { get; set; } = "";
    }
}
