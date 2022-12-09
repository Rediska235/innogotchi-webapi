using System.ComponentModel.DataAnnotations;

namespace InnoGotchi.Application.Dtos.PetModels
{
    public class PetChangeNameDto
    {
        [Required]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = "";
    }
}
