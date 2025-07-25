namespace AuthService.Application.DTOs
{
    public class AuthResponse
    {
        public string Token { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
}
