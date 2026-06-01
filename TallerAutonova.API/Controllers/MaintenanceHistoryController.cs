using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using TallerAutonova.Domain.Entities;
using TallerAutonova.Domain.Interfaces.Services;
using TallerAutonova.API.DTOs.Request;
using TallerAutonova.API.DTOs.Response;

namespace TallerAutonova.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceHistoryController : ControllerBase
    {
        private readonly IMaintenanceHistoryService _historyService;
        private readonly IMapper _mapper;

        public MaintenanceHistoryController(IMaintenanceHistoryService historyService, IMapper mapper)
        {
            _historyService = historyService;
            _mapper = mapper;
        }

        // ========== CRUD MAINTENANCE HISTORY ==========

        // GET: api/maintenancehistory
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var histories = await _historyService.GetAllAsync();
            var response = _mapper.Map<IEnumerable<MaintenanceHistoryResponseDTO>>(histories);
            return Ok(response);
        }

        // GET: api/maintenancehistory/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var history = await _historyService.GetByIdAsync(id);
            if (history == null)
                return NotFound($"Historial con ID {id} no encontrado");

            var response = _mapper.Map<MaintenanceHistoryResponseDTO>(history);
            return Ok(response);
        }

        // POST: api/maintenancehistory
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MaintenanceHistoryRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                // Usando el DTO de tus compañeros (sin ReceptionistId)
                var history = new MaintenanceHistory
                {
                    VehicleId = request.VehicleId,
                    MechanicId = request.MechanicId,
                    Observations = request.Observations,
                    Date = DateTime.UtcNow
                    // ReceptionistId se asigna desde el token/usuario logueado
                };

                var created = await _historyService.CreateAsync(history);
                var response = _mapper.Map<MaintenanceHistoryResponseDTO>(created);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/maintenancehistory/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MaintenanceHistoryRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _historyService.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Historial con ID {id} no encontrado");

            // Actualizar solo los campos permitidos
            existing.Observations = request.Observations;
            existing.VehicleId = request.VehicleId;
            existing.MechanicId = request.MechanicId;
            existing.UpdatedAt = DateTime.UtcNow;

            await _historyService.UpdateAsync(existing);
            return NoContent();
        }

        // DELETE: api/maintenancehistory/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _historyService.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Historial con ID {id} no encontrado");

            await _historyService.DeleteAsync(id);
            return NoContent();
        }

        // ========== CONSULTAS ESPECÍFICAS ==========

        // GET: api/maintenancehistory/vehicle/{vehicleId}
        [HttpGet("vehicle/{vehicleId}")]
        public async Task<IActionResult> GetByVehicle(int vehicleId)
        {
            var histories = await _historyService.GetByVehicleIdAsync(vehicleId);
            var response = _mapper.Map<IEnumerable<MaintenanceHistoryResponseDTO>>(histories);
            return Ok(response);
        }

        // GET: api/maintenancehistory/mechanic/{mechanicId}
        [HttpGet("mechanic/{mechanicId}")]
        public async Task<IActionResult> GetByMechanic(int mechanicId)
        {
            var histories = await _historyService.GetByMechanicIdAsync(mechanicId);
            var response = _mapper.Map<IEnumerable<MaintenanceHistoryResponseDTO>>(histories);
            return Ok(response);
        }

        // GET: api/maintenancehistory/date-range?start=2024-01-01&end=2024-12-31
        [HttpGet("date-range")]
        public async Task<IActionResult> GetByDateRange([FromQuery] DateTime start, [FromQuery] DateTime end)
        {
            if (start > end)
                return BadRequest("La fecha inicial no puede ser mayor a la fecha final");

            var histories = await _historyService.GetByDateRangeAsync(start, end);
            var response = _mapper.Map<IEnumerable<MaintenanceHistoryResponseDTO>>(histories);
            return Ok(response);
        }
    }
}