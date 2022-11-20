using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnoGotchi_WebApi.Models.User
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; } = "";

        [Required]
        [MaxLength(20)]
        public string LastName { get; set; } = "";

        [Required]
        [MaxLength(40)]
        public string Email { get; set; } = "";

        [MaxLength(60)]
        public string? PasswordHash { get; set; }

        [JsonIgnore]
        public Farm.Farm Farm { get; set; }
    }
}