using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_WebApi.Models.Pet
{
    public class Pet
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = "";
        
        public Farm.Farm Farm { get; set; }
        
        public int Eyes { get; set; }        
        public int Nose { get; set; }
        public int Mouth { get; set; }
        public int Ears { get; set; }
        
        public int Age { get; set; }
        public int Hunger { get; set; }
        public int Thirsty { get; set; }
        public int HappinessDays { get; set; }
        public bool isAlive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastFed { get; set; } = DateTime.Now;
        public DateTime LastDrank { get; set; } = DateTime.Now;
    }
}
