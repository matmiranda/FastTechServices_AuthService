
using static BCrypt.Net.BCrypt;

namespace AuthService.Application.Services
{
    public class AuthService : Interfaces.IAuthService
    {
        private readonly Domain.Interfaces.IUserRepository _userRepository;
        private readonly Domain.Interfaces.IJwtService _jwtService;

        public AuthService(Domain.Interfaces.IUserRepository userRepository, Domain.Interfaces.IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || !Verify(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Email ou senha inválidos.");

            return _jwtService.GenerateToken(user.Id, email);
        }
    }
}
