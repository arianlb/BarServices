using System.ComponentModel.DataAnnotations;

namespace BarServices.DTOs
{
    public class TableUpdateDTO
    {
        [Required]
        public int Number { get; set; }
    }
}
