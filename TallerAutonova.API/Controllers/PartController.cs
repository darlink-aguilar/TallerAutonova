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
    public class PartController : ControllerBase
    {
        private readonly IPartService _partService;
        private readonly IMapper _mapper;

        public PartController(IPartService partService, IMapper mapper)
        {
            _partService = partService;
            _mapper = mapper;
        }

        // ========== CRUD PART ==========

        // GET: api/part
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var parts = await _partService.GetAllAsync();
            var response = _mapper.Map<IEnumerable<PartResponseDTO>>(parts);
            return Ok(response);
        }

        // GET: api/part/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var part = await _partService.GetByIdAsync(id);
            if (part == null)
                return NotFound($"Repuesto con ID {id} no encontrado");

            var response = _mapper.Map<PartResponseDTO>(part);
            return Ok(response);
        }

        // POST: api/part
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PartRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var part = _mapper.Map<Part>(request);
                var created = await _partService.CreateAsync(part);
                var response = _mapper.Map<PartResponseDTO>(created);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/part/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PartRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _partService.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Repuesto con ID {id} no encontrado");

            _mapper.Map(request, existing);
            await _partService.UpdateAsync(existing);
            return NoContent();
        }

        // DELETE: api/part/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _partService.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Repuesto con ID {id} no encontrado");

            await _partService.DeleteAsync(id);
            return NoContent();
        }

        // ========== GESTIÓN DE STOCK (OBSERVER) ==========

        // PUT: api/part/reduce-stock
        [HttpPut("reduce-stock")]
        public async Task<IActionResult> ReduceStock([FromBody] UpdateStockRequestDTO request)
        {
            if (request.Quantity <= 0)
                return BadRequest("La cantidad debe ser mayor a 0");

            var result = await _partService.ReduceStockAsync(request.PartId, request.Quantity);
            if (!result)
                return BadRequest("No se pudo reducir el stock. Verifique que el repuesto exista y tenga stock suficiente.");

            return Ok(new { message = "Stock reducido exitosamente", partId = request.PartId, quantity = request.Quantity });
        }

        // PUT: api/part/increase-stock
        [HttpPut("increase-stock")]
        public async Task<IActionResult> IncreaseStock([FromBody] UpdateStockRequestDTO request)
        {
            if (request.Quantity <= 0)
                return BadRequest("La cantidad debe ser mayor a 0");

            var result = await _partService.IncreaseStockAsync(request.PartId, request.Quantity);
            if (!result)
                return BadRequest("No se pudo aumentar el stock. Verifique que el repuesto exista.");

            return Ok(new { message = "Stock aumentado exitosamente", partId = request.PartId, quantity = request.Quantity });
        }

        // GET: api/part/low-stock
        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStock()
        {
            var lowStockParts = await _partService.GetLowStockPartsAsync();
            var response = _mapper.Map<IEnumerable<PartResponseDTO>>(lowStockParts);
            return Ok(response);
        }

        // GET: api/part/stock/{id}
        [HttpGet("stock/{id}")]
        public async Task<IActionResult> GetStock(int id)
        {
            var stock = await _partService.GetCurrentStockAsync(id);
            return Ok(new { partId = id, currentStock = stock });
        }

        // GET: api/part/administrator/{administratorId}
        [HttpGet("administrator/{administratorId}")]
        public async Task<IActionResult> GetByAdministrator(int administratorId)
        {
            var parts = await _partService.GetPartsByAdministratorAsync(administratorId);
            var response = _mapper.Map<IEnumerable<PartResponseDTO>>(parts);
            return Ok(response);
        }
    }
}