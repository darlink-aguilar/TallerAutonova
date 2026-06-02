using AutoNova.API.DTOs.Request;
using AutoNova.API.DTOs.Response;
using AutoNova.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoNova.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService) => _authService = authService;

    /// <summary>Iniciar sesión y obtener token JWT.</summary>
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), 200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            var result = await _authService.LoginAsync(request.Email, request.Password);

            if (result is null)
                return Unauthorized(new { message = "Credenciales incorrectas o usuario inactivo." });

            var (token, user) = result.Value;

            return Ok(new LoginResponse
            {
                Token      = token,
                Role       = user.Role.ToString(),
                Name       = user.Name,
                Email      = user.Email,
                UserId     = user.Id,
                Expiration = DateTime.UtcNow.AddHours(8)
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message    = ex.Message,
                innerError = ex.InnerException?.Message,
                type       = ex.GetType().Name
            });
        }
    }
}
