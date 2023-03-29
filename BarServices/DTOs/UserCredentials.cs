using System.ComponentModel.DataAnnotations;

namespace BarServices.DTOs
{
    public class UserCredentials
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public string Password { get; set;} = null!;
    }
}
