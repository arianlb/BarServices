using Microsoft.EntityFrameworkCore;

namespace BarServices.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string Category { get; set; } = null!;
        public string Picture { get; set; } = "";
        public string Status { get; set; } = "";
        public bool Offer { get; set; }
        public DateTime? OrderTime { get; set; }
        public int? TableId { get; set; }
        public Table? Table { get; set; }
        public int? ElaborationId { get; set; }
        public Elaboration? Elaboration { get; set; }
    }
}
