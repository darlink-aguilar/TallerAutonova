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
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IMapper             _mapper;

    public AppointmentController(IAppointmentService appointmentService, IMapper mapper)
    {
        _appointmentService = appointmentService;
        _mapper             = mapper;
    }

    /// <summary>Listar todas las citas.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AppointmentResponse>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var appointments = await _appointmentService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<AppointmentResponse>>(appointments));
    }

    /// <summary>Obtener cita por ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(AppointmentResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            return Ok(_mapper.Map<AppointmentResponse>(appointment));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Citas por vehículo.</summary>
    [HttpGet("vehicle/{vehicleId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<AppointmentResponse>), 200)]
    public async Task<IActionResult> GetByVehicle(Guid vehicleId)
    {
        var appointments = await _appointmentService.GetByVehicleIdAsync(vehicleId);
        return Ok(_mapper.Map<IEnumerable<AppointmentResponse>>(appointments));
    }

    /// <summary>Citas por mecánico.</summary>
    [HttpGet("mechanic/{mechanicId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<AppointmentResponse>), 200)]
    public async Task<IActionResult> GetByMechanic(Guid mechanicId)
    {
        var appointments = await _appointmentService.GetByMechanicIdAsync(mechanicId);
        return Ok(_mapper.Map<IEnumerable<AppointmentResponse>>(appointments));
    }

    /// <summary>Crear nueva cita (Administrador / Recepcionista).</summary>
    [HttpPost]
    [Authorize(Roles = "Administrador,Recepcionista")]
    [ProducesResponseType(typeof(AppointmentResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentRequest request)
    {
        try
        {
            var appointment = await _appointmentService.CreateAsync(
                request.Date, request.Time, request.Description,
                request.VehicleId, request.MechanicId);
            return CreatedAtAction(nameof(GetById), new { id = appointment.Id },
                _mapper.Map<AppointmentResponse>(appointment));
        }
        catch (KeyNotFoundException ex)    { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    /// <summary>Actualizar cita (Administrador / Recepcionista).</summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Administrador,Recepcionista")]
    [ProducesResponseType(typeof(AppointmentResponse), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAppointmentRequest request)
    {
        try
        {
            var appointment = await _appointmentService.UpdateAsync(id,
                request.Date, request.Time, request.Description,
                request.VehicleId, request.MechanicId);
            return Ok(_mapper.Map<AppointmentResponse>(appointment));
        }
        catch (KeyNotFoundException ex)    { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    /// <summary>Cambiar estado de la cita (State Pattern: start | complete | cancel).</summary>
    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(AppointmentResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ChangeStatus(Guid id, [FromBody] ChangeAppointmentStatusRequest request)
    {
        try
        {
            switch (request.Action.ToLower())
            {
                case "start":    await _appointmentService.StartAsync(id);    break;
                case "complete": await _appointmentService.CompleteAsync(id); break;
                case "cancel":   await _appointmentService.CancelAsync(id);   break;
                default:
                    return BadRequest(new { message = "Acción inválida. Use: start | complete | cancel" });
            }
            var updated = await _appointmentService.GetByIdAsync(id);
            return Ok(_mapper.Map<AppointmentResponse>(updated));
        }
        catch (KeyNotFoundException ex)    { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    /// <summary>Cancelar cita (Administrador / Recepcionista).</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Administrador,Recepcionista")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Cancel(Guid id)
    {
        try
        {
            await _appointmentService.CancelAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)    { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }
}
