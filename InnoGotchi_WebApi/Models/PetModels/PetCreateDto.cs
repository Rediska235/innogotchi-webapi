using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_WebApi.Models.PetModels
{
    public class PetCreateDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = "";

        [Required]
        public int Eyes { get; set; }

        [Required]
        public int Nose { get; set; }

        [Required]
        public int Mouth { get; set; }

        [Required]
        public int Ears { get; set; }
    }
}
