using System.ComponentModel.DataAnnotations;

namespace BarServices.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Roles { get; set; }
    }
}
