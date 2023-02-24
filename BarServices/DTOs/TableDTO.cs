using BarServices.Models;

namespace BarServices.DTOs
{
    public class TableDTO
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
        public ElaborationDTO Bar { get; set; } = null!;
        public ElaborationDTO Kitchen { get; set; } = null!;
    }
}
