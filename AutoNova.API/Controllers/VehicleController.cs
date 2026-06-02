using AutoMapper;
using AutoNova.API.DTOs.Request;
using AutoNova.API.DTOs.Response;
using AutoNova.Domain.Entities;
using AutoNova.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AutoNova.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;
    private readonly IMapper         _mapper;

    public VehicleController(IVehicleService vehicleService, IMapper mapper)
    {
        _vehicleService = vehicleService;
        _mapper         = mapper;
    }

    /// <summary>Listar todos los vehículos.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VehicleResponse>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var vehicles = await _vehicleService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<VehicleResponse>>(vehicles));
    }

    /// <summary>Obtener vehículo por ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VehicleResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var vehicle = await _vehicleService.GetByIdAsync(id);
            return Ok(_mapper.Map<VehicleResponse>(vehicle));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Obtener vehículo por placa.</summary>
    [HttpGet("plate/{plate}")]
    [ProducesResponseType(typeof(VehicleResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetByPlate(string plate)
    {
        try
        {
            var vehicle = await _vehicleService.GetByPlateAsync(plate);
            return Ok(_mapper.Map<VehicleResponse>(vehicle));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Registrar vehículo con propietario (Administrador / Recepcionista).</summary>
    [HttpPost]
    [Authorize(Roles = "Administrador,Recepcionista")]
    [ProducesResponseType(typeof(VehicleResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Create([FromBody] CreateVehicleWithOwnerRequest request)
    {
        try
        {
            var vehicle = new Vehicle
            {
                Plate = request.Plate,
                Brand = request.Brand,
                Model = request.Model,
                Year  = request.Year,
                Color = request.Color
            };

            var owner = new Owner
            {
                FullName       = request.OwnerFullName,
                DocumentNumber = request.OwnerDocumentNumber,
                Email          = request.OwnerEmail,
                Phone          = request.OwnerPhone,
                Address        = request.OwnerAddress
            };

            var created = await _vehicleService.CreateAsync(vehicle, owner);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<VehicleResponse>(created));
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex)         { return BadRequest(new { message = ex.Message }); }
    }

    /// <summary>Actualizar vehículo (Administrador / Recepcionista).</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Administrador,Recepcionista")]
    [ProducesResponseType(typeof(VehicleResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVehicleRequest request)
    {
        try
        {
            var vehicle = await _vehicleService.UpdateAsync(id, request.Plate, request.Brand,
                request.Model, request.Year, request.Color);
            return Ok(_mapper.Map<VehicleResponse>(vehicle));
        }
        catch (KeyNotFoundException ex)    { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex)         { return BadRequest(new { message = ex.Message }); }
    }

    /// <summary>Desactivar vehículo (Administrador).</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        try
        {
            await _vehicleService.DeactivateAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Activar vehículo (Administrador).</summary>
    [HttpPatch("{id:guid}/activate")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Activate(Guid id)
    {
        try
        {
            await _vehicleService.ActivateAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}
