using System.ComponentModel.DataAnnotations;

namespace InnoGotchi.Application.Dtos.FarmModels
{
    public class FarmCreateDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = "";
    }
}
