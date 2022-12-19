namespace InnoGotchi.Domain.Entities
{
    public class FriendFarm
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public int FarmId { get; set; }
        public Farm Farm { get; set; }
    }
}
