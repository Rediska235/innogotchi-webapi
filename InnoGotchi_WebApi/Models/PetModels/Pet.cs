using InnoGotchi_WebApi.Models.FarmModels;
using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_WebApi.Models.PetModels
{
    public class Pet
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = "";
        
        public Farm Farm { get; set; }
        
        public int Eyes { get; set; }        
        public int Nose { get; set; }
        public int Mouth { get; set; }
        public int Ears { get; set; }
        
        public int Age { get; set; }
        public HungerLevel Hunger { get; set; }
        public ThirstLevel Thirst { get; set; }
        public int HappinessDays { get; set; }
        public bool isAlive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LastFed { get; set; } = DateTime.Now;
        public DateTime LastDrank { get; set; } = DateTime.Now;



        // F - for food
        private const int F_NORMAL_LIMIT = 1;
        private const int F_HUNGER_LIMIT = 2;
        private const int F_DEAD_LIMIT = 3;

        // D - for drink
        private const int D_NORMAL_LIMIT = 1;
        private const int D_THIRST_LIMIT = 2;
        private const int D_DEAD_LIMIT = 3;

        private const int DAYS_TO_PET_YEARS = 1;
        
        private void SetVitalSigns()
        {
            double hungerDifference = (DateTime.Now - LastFed).TotalDays;
            if (hungerDifference >= F_NORMAL_LIMIT && hungerDifference < F_HUNGER_LIMIT)
            {
                Hunger = HungerLevel.Normal;
            }
            else if (hungerDifference >= F_HUNGER_LIMIT && hungerDifference < F_DEAD_LIMIT)
            {
                Hunger = HungerLevel.Hungry;
            }
            else if (hungerDifference >= F_DEAD_LIMIT)
            {
                Hunger = HungerLevel.Dead;
                isAlive = false;
            }


            double thirstDifference = (DateTime.Now - LastDrank).TotalDays;
            if (thirstDifference >= D_NORMAL_LIMIT && thirstDifference < D_THIRST_LIMIT)
            {
                Thirst = ThirstLevel.Normal;
            }
            else if (thirstDifference >= D_THIRST_LIMIT && thirstDifference < D_DEAD_LIMIT)
            {
                Thirst = ThirstLevel.Thirsty;
            }
            else if (thirstDifference >= D_DEAD_LIMIT)
            {
                Thirst = ThirstLevel.Dead;
                isAlive = false;
            }

            Age = (int)(DateTime.Now - CreatedAt).TotalDays / DAYS_TO_PET_YEARS;
        }
    }
}
