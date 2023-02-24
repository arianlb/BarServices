using System.ComponentModel.DataAnnotations;

namespace BarServices.DTOs
{
    public class TableCreationDTO
    {
        [Required]
        public int Number { get; set; }
        [Required]
        public int BarId { get; set; }
        [Required]
        public int KitchenId { get; set; }
    }
}
