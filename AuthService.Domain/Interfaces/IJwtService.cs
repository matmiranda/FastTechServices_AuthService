using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(ulong userId, string email, UserRole role, string? position = null);
    }
}
