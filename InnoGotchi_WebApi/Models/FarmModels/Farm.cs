using InnoGotchi_WebApi.Models.UserModels;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnoGotchi_WebApi.Models.FarmModels
{
    public class Farm
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [JsonIgnore]
        public int UserId { get; set; }
        
        [JsonIgnore]
        public User? User { get; set; }
    }
}
