
namespace AuthService.Domain.Entities
{
    public class User
    {
        public ulong Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Cpf { get; set; }
        public string Password_Hash { get; set; } = string.Empty;
        public UserRole Role_Id { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Updated_At { get; set; }
    }
}
