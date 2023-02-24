using System.ComponentModel.DataAnnotations;

namespace BarServices.DTOs
{
    public class UserUpdateDTO
    {
        public string Name { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Password { get; set; } = null!; 
        public string Roles { get; set; } = null!;
    }
}
