using AuthService.Domain.Entities;

namespace AuthService.Application.DTOs
{
    public class UserResponse
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
