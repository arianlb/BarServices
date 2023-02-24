
namespace BarServices.Models
{
    public class Elaboration
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Product> Products { get; set; } = new List<Product>();

    }
}
