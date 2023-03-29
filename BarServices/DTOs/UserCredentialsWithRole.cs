namespace BarServices.DTOs
{
    public class UserCredentialsWithRole : UserCredentials
    {
        public string Role { get; set; } = null!;
    }
}
