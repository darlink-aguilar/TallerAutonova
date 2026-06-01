using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TallerAutonova.API.DTOs.Request;
using TallerAutonova.API.DTOs.Response;
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Interfaces.Services;

namespace TallerAutonova.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MechanicController : ControllerBase
    {
        private readonly IMechanicService _mechanicService;
        private readonly IMapper _mapper;
        private readonly ILogger<MechanicController> _logger;

        public MechanicController(
            IMechanicService mechanicService,
            IMapper mapper,
            ILogger<MechanicController> logger)
        {
            _mechanicService = mechanicService;
            _mapper = mapper;
            _logger = logger;
        }

        
        // Añadir una nueva observación al historial de mantenimiento de un vehículo.
        [HttpPost("observations")] 
        public async Task<ActionResult<MaintenanceHistoryResponseDTO>> AddObservation(MaintenanceHistoryRequestDTO dto)
        {
            try
            {
                _logger.LogInformation("Recibida solicitud para añadir observación al vehículo ID: {VehicleId}", dto.VehicleId);

                var maintenanceHistory = _mapper.Map<MaintenanceHistory>(dto);
                var createdRecord = await _mechanicService.AddObservationAsync(maintenanceHistory);
                var responseDto = _mapper.Map<MaintenanceHistoryResponseDTO>(createdRecord);

                return CreatedAtAction(
                    nameof(GetHistoryByVehicleId),
                    new { vehicleId = responseDto.VehicleId },
                    responseDto);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Validación fallida al intentar registrar observación: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning("Entidad no encontrada al registrar observación: {Message}", ex.Message);
                return NotFound(new { message = ex.Message });
            }
        }

        // Consulta el historial 
        [HttpGet("vehicles/{vehicleId}/history")] 
        public async Task<ActionResult<IEnumerable<MaintenanceHistoryResponseDTO>>> GetHistoryByVehicleId(int vehicleId)
        {
            try
            {
                _logger.LogInformation("Consultando historial para el vehículo ID: {VehicleId}", vehicleId);

                var history = await _mechanicService.GetHistoryByVehicleIdAsync(vehicleId);
                var historyDto = _mapper.Map<IEnumerable<MaintenanceHistoryResponseDTO>>(history);

                return Ok(historyDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        // Consulta todas las citas asignadas 
        [HttpGet("{mechanicId}/appointments")] 
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAppointmentsByMechanicId(int mechanicId)
        {
            _logger.LogInformation("Consultando citas asignadas al mecánico ID: {MechanicId}", mechanicId);
            var appointments = await _mechanicService.GetAppointmentsByMechanicIdAsync(mechanicId);

            return Ok(appointments);
        }
    }
}