using System.ComponentModel.DataAnnotations;

namespace BarServices.DTOs
{
    public class ProductCreationDTO
    {
        [Required]
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        [Required]
        public string Category { get; set; } = null!;
        public bool Offer { get; set; }
    }
}
