using System.ComponentModel.DataAnnotations;

namespace BarServices.DTOs
{
    public class UserCreationDTO
    {
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
