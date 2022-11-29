using InnoGotchi_WebApi.Models.FarmModels;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnoGotchi_WebApi.Models.UserModels
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(20)]
        public string FirstName { get; set; } = "";

        [MaxLength(20)]
        public string LastName { get; set; } = "";
        
        [MaxLength(40)]
        public string Email { get; set; } = "";

        public DateTime CreatedAt { get; set; }

        public int PasswordLength { get; set; }

        [JsonIgnore]
        [MaxLength(60)]
        public string? PasswordHash { get; set; } = "";

        [JsonIgnore]
        public Farm? Farm { get; set; }

        [JsonIgnore]
        public List<FriendFarm> FriendsFarms { get; set; }
    }
}