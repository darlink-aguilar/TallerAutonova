using AutoMapper;
using AutoNova.API.DTOs.Request;
using AutoNova.API.DTOs.Response;
using AutoNova.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoNova.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OwnerController : ControllerBase
{
    private readonly IOwnerService _ownerService;
    private readonly IMapper       _mapper;

    public OwnerController(IOwnerService ownerService, IMapper mapper)
    {
        _ownerService = ownerService;
        _mapper       = mapper;
    }

    /// <summary>Listar todos los propietarios.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OwnerResponse>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var owners = await _ownerService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<OwnerResponse>>(owners));
    }

    /// <summary>Obtener propietario por ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OwnerResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var owner = await _ownerService.GetByIdAsync(id);
            return Ok(_mapper.Map<OwnerResponse>(owner));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Obtener propietario por vehículo.</summary>
    [HttpGet("vehicle/{vehicleId:guid}")]
    [ProducesResponseType(typeof(OwnerResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByVehicleId(Guid vehicleId)
    {
        try
        {
            var owner = await _ownerService.GetByVehicleIdAsync(vehicleId);
            return Ok(_mapper.Map<OwnerResponse>(owner));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Actualizar datos del propietario (Administrador / Recepcionista).</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Administrador,Recepcionista")]
    [ProducesResponseType(typeof(OwnerResponse), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateOwnerRequest request)
    {
        try
        {
            var owner = await _ownerService.UpdateAsync(id, request.FullName, request.DocumentNumber,
                request.Email, request.Phone, request.Address);
            return Ok(_mapper.Map<OwnerResponse>(owner));
        }
        catch (KeyNotFoundException ex)    { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }
}
