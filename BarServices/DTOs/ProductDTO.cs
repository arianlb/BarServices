namespace BarServices.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Category { get; set; } = null!;
        public string Picture { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
