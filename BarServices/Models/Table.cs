
using System.Runtime.Serialization;

namespace BarServices.Models
{
    public class Table
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
        public Elaboration? Bar { get; set; }
        public Elaboration? Kitchen { get; set; }
    }
}
