using System.ComponentModel.DataAnnotations;

namespace InnoGotchi.Application.Dtos.PetModels
{
    public class PetCreateDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = "";

        [Required]
        [Range(1, 6)]
        public int Eyes { get; set; }

        [Required]
        [Range(1, 6)]
        public int Nose { get; set; }

        [Required]
        [Range(1, 5)]
        public int Mouth { get; set; }

        [Required]
        [Range(1, 5)]
        public int Body { get; set; }
    }
}
