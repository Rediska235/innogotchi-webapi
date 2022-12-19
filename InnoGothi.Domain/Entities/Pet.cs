using InnoGotchi.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InnoGotchi.Domain.Entities
{
    public class Pet
    {
        public int Id { get; set; }
        
        [MaxLength(20)]
        public string Name { get; set; } = "";

        [JsonIgnore]
        public int? FarmId { get; set; }
        public Farm Farm { get; set; }

        // JsonIgnore for now
        [JsonIgnore]
        public int Eyes { get; set; }
        [JsonIgnore]
        public int Nose { get; set; }
        [JsonIgnore]
        public int Mouth { get; set; }
        [JsonIgnore]
        public int Body { get; set; }
        
        public int Age { get; set; }
        public HungerLevel Hunger { get; set; }
        public ThirstLevel Thirst { get; set; }
        public int HappinessDays { get; set; }
        public bool IsAlive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastFed { get; set; } = DateTime.Now;
        public DateTime LastDrank { get; set; } = DateTime.Now;
        public DateTime HappinessDaysStart { get; set; } = DateTime.Now;

        // F - for food
        private const int F_NORMAL_LIMIT = 1;
        private const int F_HUNGER_LIMIT = 2;
        private const int F_DEAD_LIMIT = 3;

        // D - for drink
        private const int D_NORMAL_LIMIT = 1;
        private const int D_THIRST_LIMIT = 2;
        private const int D_DEAD_LIMIT = 3;

        private const int DAYS_TO_PET_YEARS = 1;
        
        public void SetVitalSigns()
        {
            double hungerDifference = (DateTime.Now - LastFed).TotalDays;
            if (hungerDifference < F_NORMAL_LIMIT)
            {
                Hunger = HungerLevel.Full;
            }
            else if (hungerDifference >= F_NORMAL_LIMIT && hungerDifference < F_HUNGER_LIMIT)
            {
                Hunger = HungerLevel.Normal;
            }
            else if (hungerDifference >= F_HUNGER_LIMIT && hungerDifference < F_DEAD_LIMIT)
            {
                Hunger = HungerLevel.Hungry;
            }
            else
            {
                Hunger = HungerLevel.Dead;
                IsAlive = false;
            }


            double thirstDifference = (DateTime.Now - LastDrank).TotalDays;
            if(thirstDifference < D_NORMAL_LIMIT)
            {
                Thirst = ThirstLevel.Full;
            }
            else if (thirstDifference >= D_NORMAL_LIMIT && thirstDifference < D_THIRST_LIMIT)
            {
                Thirst = ThirstLevel.Normal;
            }
            else if (thirstDifference >= D_THIRST_LIMIT && thirstDifference < D_DEAD_LIMIT)
            {
                Thirst = ThirstLevel.Thirsty;
            }
            else
            {
                Thirst = ThirstLevel.Dead;
                IsAlive = false;
            }
            
            Age = (int)(DateTime.Now - CreatedAt).TotalDays / DAYS_TO_PET_YEARS;

            if (Thirst >= ThirstLevel.Normal && Hunger >= HungerLevel.Normal)
            {
                HappinessDays = (int)(DateTime.Now - HappinessDaysStart).TotalDays;
            }
            else
            {
                HappinessDays = 0;
                HappinessDaysStart = DateTime.Now;
            }
        }
    }
}
