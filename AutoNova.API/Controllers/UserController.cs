using AutoMapper;
using AutoNova.API.DTOs.Request;
using AutoNova.API.DTOs.Response;
using AutoNova.Domain.Enums;
using AutoNova.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoNova.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]  // clase base: cualquier usuario autenticado
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper      _mapper;

    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper      = mapper;
    }

    /// <summary>Mecánicos activos — accesible para todos los roles (dropdown de citas).</summary>
    [HttpGet("mechanics")]
    [ProducesResponseType(typeof(IEnumerable<UserResponse>), 200)]
    public async Task<IActionResult> GetMechanics()
    {
        var all = await _userService.GetAllAsync();
        var mechanics = all.Where(u => u.Role == UserRole.Mecanico && u.IsActive);
        return Ok(_mapper.Map<IEnumerable<UserResponse>>(mechanics));
    }

    // ── Endpoints exclusivos del Administrador ────────────────────────────────

    /// <summary>Listar todos los usuarios.</summary>
    [HttpGet]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(IEnumerable<UserResponse>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<UserResponse>>(users));
    }

    /// <summary>Obtener usuario por ID.</summary>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(UserResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(_mapper.Map<UserResponse>(user));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Crear nuevo usuario (solo Administrador — Factory Method: Mecánico o Recepcionista).</summary>
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(UserResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        try
        {
            var user = await _userService.CreateAsync(
                request.Name, request.Email, request.Password, request.Role);
            return CreatedAtAction(nameof(GetById), new { id = user.Id },
                _mapper.Map<UserResponse>(user));
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    /// <summary>Actualizar usuario.</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(UserResponse), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateUserRequest request)
    {
        try
        {
            var user = await _userService.UpdateAsync(id, request.Name, request.Email, request.Role);
            return Ok(_mapper.Map<UserResponse>(user));
        }
        catch (KeyNotFoundException ex)      { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    /// <summary>Desactivar usuario.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        try
        {
            await _userService.DeactivateAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Activar usuario.</summary>
    [HttpPatch("{id:guid}/activate")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Activate(Guid id)
    {
        try
        {
            await _userService.ActivateAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Cambiar contraseña.</summary>
    [HttpPatch("{id:guid}/password")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ChangePassword(Guid id, [FromBody] ChangePasswordRequest request)
    {
        try
        {
            await _userService.ChangePasswordAsync(id, request.NewPassword);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}
