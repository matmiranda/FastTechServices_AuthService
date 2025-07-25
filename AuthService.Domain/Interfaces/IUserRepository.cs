
using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task AddUserAsync(User user);

        Task<bool> DeleteUserAsync(ulong id);
        Task<User?> GetByIdAsync(ulong id);
    }
}
