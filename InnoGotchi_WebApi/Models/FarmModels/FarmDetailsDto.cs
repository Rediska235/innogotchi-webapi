namespace InnoGotchi_WebApi.Models.FarmModels
{
    public class FarmDetailsDto
    {
        public string Name { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public int PetCount { get; set; }
        public int AliveCount { get; set; }
        public int DeadCount { get; set; }
    }
}
