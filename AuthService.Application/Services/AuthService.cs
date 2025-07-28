using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static BCrypt.Net.BCrypt;

namespace AuthService.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<IActionResult> RegisterUserAsync(RegisterRequest request)
        {
            try
            {
                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    Cpf = request.Cpf,
                    Password_Hash = HashPassword(request.Password),
                    Role_Id = request.Role,
                    Created_At = DateTime.Now
                };

                await _userRepository.AddUserAsync(user);

                var response = new UserResponse
                {
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role_Id,
                    CreatedAt = user.Created_At
                };

                return new OkObjectResult(response);
            }
            catch (ApplicationException ex)
            {
                return new BadRequestObjectResult(new { message = ex.Message });
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> DeleteUserAsync(ulong id)
        {
            try
            {
                var deleted = await _userRepository.DeleteUserAsync(id);

                if (!deleted)
                    return new NotFoundObjectResult(new { message = "Usuário não encontrado ou já removido." });

                return new OkObjectResult(new { message = "Usuário removido com sucesso." });
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        public async Task<IActionResult> GetUserByIdAsync(ulong id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);

                if (user == null)
                    return new NotFoundObjectResult(new { message = "Usuário não encontrado." });

                var response = new UserResponse
                {
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role_Id,
                    CreatedAt = user.Created_At
                };

                return new OkObjectResult(response);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }


        public async Task<AuthResponse> LoginAsync(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || !Verify(password, user.Password_Hash))
                throw new UnauthorizedAccessException("Email ou senha inválidos.");

            var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role_Id);

            return new AuthResponse
            {
                Token = token,
                Name = user.Name,
                Role = user.Role_Id.ToString()
            };
        }

        public async Task<IActionResult> LoginAsync(LoginRequest login)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(login.Email);

                if (user == null || !Verify(login.Password, user.Password_Hash))
                    return new UnauthorizedObjectResult(new { message = "Credenciais inválidas." });

                var token = _jwtService.GenerateToken(user.Id, user.Email, user.Role_Id);

                return new OkObjectResult(new
                {
                    message = "Login bem-sucedido",
                    token,
                    user = new
                    {
                        user.Id,
                        user.Name,
                        user.Email,
                        roleId = user.Role_Id
                    }
                });
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        public Task<IActionResult> ValidateAsync(ClaimsPrincipal user)
        {
            var email = user.FindFirst(ClaimTypes.Email)?.Value;
            var role = user.FindFirst(ClaimTypes.Role)?.Value;

            return Task.FromResult<IActionResult>(new OkObjectResult(new { message = "Token válido", email, role }));
        }
    }
}
