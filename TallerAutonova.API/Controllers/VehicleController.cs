using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TallerAutonova.API.DTOs.Request;
using TallerAutonova.API.DTOs.Response;
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Interfaces.Services;
using TallerAutonova.Domain.Services;
namespace TallerAutonova.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;
    private readonly IMapper _mapper;

    public VehicleController(
    IVehicleService vehicleService,
    IMapper mapper)
    {
        _vehicleService = vehicleService;
        _mapper = mapper;
    }

    [HttpGet("{vehicleId}")]
    public async Task<ActionResult<VehicleResponseDTO>> GetById(int vehicleId)
    {
        var vehicle = await _vehicleService.GetByIdAsync(vehicleId);
        if (vehicle == null)
            return NotFound(new { message = $"Vehículo con ID {vehicleId} no encontrado" });
        return Ok(_mapper.Map<VehicleResponseDTO>(vehicle));
    }

    [HttpGet]
    public async Task<ActionResult<VehicleResponseDTO>> GetAll()
    {
        var vehicles = await _vehicleService.GetAllAsync();
        if (vehicles == null)
            return NotFound(new { message = $"No hay registro de vehículos" });
        return Ok(_mapper.Map<VehicleResponseDTO>(vehicles));
    }

    [HttpPost]
    public async Task<ActionResult<VehicleResponseDTO>> Create(VehicleRequestDTO dto)
    {
        try
        {
            var vehicle = _mapper.Map<Vehicle>(dto);
            var created = await _vehicleService.CreateAsync(vehicle, dto.OwnerName, dto.OwnerPhone);
            var responseDto = _mapper.Map<VehicleResponseDTO>(created);
            return CreatedAtAction(nameof(GetById), new { id = responseDto.Id }, responseDto);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, SponsorRequestDTO dto)
    {
        try
        {
            var vehicle = _mapper.Map<Vehicle>(dto);
            await _vehicleService.UpdateAsync(id, vehicle);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            await _vehicleService.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
    }

    [HttpGet("{id}/tournament")]
    public async Task<ActionResult<IEnumerable<TournamentResponseDTO>>>> GetTournaments(int id)
    {
        try
        {
            var tournaments = await _sponsorService.GetTournamentsBySponsorAsync(id);
            return Ok(_mapper.Map<IEnumerable<SponsorResponseDTO>>(tournaments));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }
}