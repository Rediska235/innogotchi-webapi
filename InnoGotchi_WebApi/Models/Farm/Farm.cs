using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnoGotchi_WebApi.Models.Farm
{
    public class Farm
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = "";

        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public User.User? User { get; set; }
    }
}
