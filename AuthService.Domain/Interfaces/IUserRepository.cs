
using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Entities.User?> GetByEmailAsync(string email);

        Task AddUserAsync(Entities.User user);

        Task<bool> DeleteUserAsync(Guid id);
        Task<User?> GetByIdAsync(Guid id);
    }
}
