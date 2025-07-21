namespace AuthService.Application.DTOs
{
    public class UserResponse
    {
        public Guid Id { get; set; }  // ou ulong, dependendo do seu banco
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
