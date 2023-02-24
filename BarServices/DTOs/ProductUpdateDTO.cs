namespace BarServices.DTOs
{
    public class ProductUpdateDTO
    {
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Category { get; set; } = null!;
        public string Status { get; set; } = null!;
        public bool Offer { get; set; }
    }
}
