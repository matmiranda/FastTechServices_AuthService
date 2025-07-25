using AuthService.Application.DTOs;
using AuthService.Application.Interfaces;
using AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(
        IUserRepository userRepository, 
        IJwtService jwtService,
        IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        return await _authService.RegisterUserAsync(request);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "GERENTE")]
    public async Task<IActionResult> Delete(ulong id)
    {
        return await _authService.DeleteUserAsync(id);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        return await _authService.LoginAsync(login);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "GERENTE")]
    public async Task<IActionResult> GetById(ulong id)
    {
        return await _authService.GetUserByIdAsync(id);
    }

    [HttpGet("validate")]
    [Authorize]
    public async Task<IActionResult> Validate()
    {
        return await _authService.ValidateAsync(User);
    }
}
