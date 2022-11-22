using InnoGotchi_WebApi.Models.FarmModels;
using InnoGotchi_WebApi.Models.UserModels;

namespace InnoGotchi_WebApi.Models
{
    public class FriendFarm
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int FarmId { get; set; }
        public Farm Farm { get; set; }
    }
}
