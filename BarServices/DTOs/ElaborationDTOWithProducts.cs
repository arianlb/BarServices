namespace BarServices.DTOs
{
    public class ElaborationDTOWithProducts : ElaborationDTO
    {
        public List<ProductDTO> Products { get; set; } = new List<ProductDTO>();
    }
}
