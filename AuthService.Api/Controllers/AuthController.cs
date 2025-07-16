using AuthService.Application.DTOs;
using AuthService.Domain.Entities;
using AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static BCrypt.Net.BCrypt;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public AuthController(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Cpf = request.Cpf,
                PasswordHash = HashPassword(request.Password),
                Role = request.Role,
                Position = request.Position,
                CreatedAt = DateTime.Now
            };

            await _userRepository.AddUserAsync(user);

            var response = new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };

            return Ok(response);
        }
        catch (ApplicationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            // Aqui você pode logar o erro se quiser
            return StatusCode(500, new { error = "Erro interno no servidor." });
        }
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest login)
    {
        var user = await _userRepository.GetByEmailAsync(login.Email);

        if (user == null || !Verify(login.Password, user.PasswordHash))
            return Unauthorized("Credenciais inválidas.");

        var token = _jwtService.GenerateToken(user.Id, user.Email);

        return Ok(new
        {
            message = "Login bem-sucedido",
            token,
            user = new
            {
                user.Id,
                user.Name,
                user.Email,
                user.Role
            }
        });
    }


    [HttpGet("validate")]
    [Authorize]
    public IActionResult Validate()
    {
        return Ok();
    }
}
