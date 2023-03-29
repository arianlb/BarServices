namespace BarServices.DTOs
{
    public class TableDTOWithProducts : TableDTO
    {
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
        public ElaborationDTO Bar { get; set; } = null!;
        public ElaborationDTO Kitchen { get; set; } = null!;
    }
}
