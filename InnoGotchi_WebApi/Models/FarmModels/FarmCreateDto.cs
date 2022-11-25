using System.ComponentModel.DataAnnotations;

namespace InnoGotchi_WebApi.Models.FarmModels
{
    public class FarmCreateDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = "";
    }
}
