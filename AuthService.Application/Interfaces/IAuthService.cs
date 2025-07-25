using AuthService.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuthService.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(string email, string password);
        Task<IActionResult> RegisterUserAsync(RegisterRequest request);
        Task<IActionResult> DeleteUserAsync(ulong id);
        Task<IActionResult> GetUserByIdAsync(ulong id);
        Task<IActionResult> LoginAsync(LoginRequest login);
        Task<IActionResult> ValidateAsync(ClaimsPrincipal user);
    }
}
