
namespace AuthService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<Entities.User?> GetByEmailAsync(string email);

        Task AddUserAsync(Entities.User user);
    }
}
