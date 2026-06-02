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
public class MaintenanceHistoryController : ControllerBase
{
    private readonly IMaintenanceHistoryService _historyService;
    private readonly IMapper                   _mapper;

    public MaintenanceHistoryController(IMaintenanceHistoryService historyService, IMapper mapper)
    {
        _historyService = historyService;
        _mapper         = mapper;
    }

    /// <summary>Listar todo el historial.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<MaintenanceHistoryResponse>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var history = await _historyService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<MaintenanceHistoryResponse>>(history));
    }

    /// <summary>Obtener registro por ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(MaintenanceHistoryResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var history = await _historyService.GetByIdAsync(id);
            return Ok(_mapper.Map<MaintenanceHistoryResponse>(history));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Historial por vehículo ID.</summary>
    [HttpGet("vehicle/{vehicleId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<MaintenanceHistoryResponse>), 200)]
    public async Task<IActionResult> GetByVehicle(Guid vehicleId)
    {
        var history = await _historyService.GetByVehicleIdAsync(vehicleId);
        return Ok(_mapper.Map<IEnumerable<MaintenanceHistoryResponse>>(history));
    }

    /// <summary>Historial por placa del vehículo.</summary>
    [HttpGet("plate/{plate}")]
    [ProducesResponseType(typeof(IEnumerable<MaintenanceHistoryResponse>), 200)]
    public async Task<IActionResult> GetByPlate(string plate)
    {
        var history = await _historyService.GetByPlateAsync(plate);
        return Ok(_mapper.Map<IEnumerable<MaintenanceHistoryResponse>>(history));
    }

    /// <summary>Registrar servicio / observación (Mecánico / Administrador).</summary>
    [HttpPost]
    [Authorize(Roles = "Administrador,Mecanico")]
    [ProducesResponseType(typeof(MaintenanceHistoryResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Create([FromBody] CreateMaintenanceHistoryRequest request)
    {
        try
        {
            var history = await _historyService.AddAsync(
                request.VehicleId, request.Observation,
                request.ServicePerformed, request.MechanicId);

            return CreatedAtAction(nameof(GetById), new { id = history.Id },
                _mapper.Map<MaintenanceHistoryResponse>(history));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Eliminar historial — no permitido por regla de negocio.</summary>
    [HttpDelete("{id:guid}")]
    public IActionResult Delete(Guid id) =>
        StatusCode(405, new { message = "El historial de mantenimiento no puede eliminarse." });
}
