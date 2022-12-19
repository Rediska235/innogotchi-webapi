using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnoGotchi.Domain.Entities
{
    public class Farm
    {
        public int Id { get; set; }
        
        [MaxLength(20)]
        public string Name { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }
        
        [JsonIgnore]
        public List<Pet> Pets { get; set; }

        [JsonIgnore]
        public List<FriendFarm> Friends { get; set; }
    }
}
