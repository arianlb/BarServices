using System.ComponentModel.DataAnnotations;

namespace BarServices.DTOs
{
    public class ElaborationCreationDTO
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}
