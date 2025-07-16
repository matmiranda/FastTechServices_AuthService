namespace AuthService.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Guid userId, string email);
    }
}
