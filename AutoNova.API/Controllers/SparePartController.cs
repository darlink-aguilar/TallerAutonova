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
public class SparePartController : ControllerBase
{
    private readonly ISparePartService _sparePartService;
    private readonly IMapper           _mapper;

    public SparePartController(ISparePartService sparePartService, IMapper mapper)
    {
        _sparePartService = sparePartService;
        _mapper           = mapper;
    }

    /// <summary>Listar todo el inventario.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SparePartResponse>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var parts = await _sparePartService.GetAllAsync();
        return Ok(_mapper.Map<IEnumerable<SparePartResponse>>(parts));
    }

    /// <summary>Obtener repuesto por ID.</summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(SparePartResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var part = await _sparePartService.GetByIdAsync(id);
            return Ok(_mapper.Map<SparePartResponse>(part));
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
    }

    /// <summary>Repuestos con stock bajo (Observer Pattern).</summary>
    [HttpGet("low-stock")]
    [ProducesResponseType(typeof(IEnumerable<SparePartResponse>), 200)]
    public async Task<IActionResult> GetLowStock()
    {
        var parts = await _sparePartService.GetLowStockAsync();
        return Ok(_mapper.Map<IEnumerable<SparePartResponse>>(parts));
    }

    /// <summary>Alertas de stock bajo (Observer Pattern).</summary>
    [HttpGet("alerts")]
    [ProducesResponseType(typeof(IReadOnlyList<string>), 200)]
    public async Task<IActionResult> GetAlerts()
    {
        var alerts = await _sparePartService.GetAlertsAsync();
        return Ok(new { alerts, count = alerts.Count });
    }

    /// <summary>Crear repuesto (Administrador).</summary>
    [HttpPost]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(StockOperationResponse), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> Create([FromBody] CreateSparePartRequest request)
    {
        try
        {
            var (part, alerts) = await _sparePartService.CreateAsync(
                request.Code, request.Name, request.Quantity, request.MinimumStock);

            return CreatedAtAction(nameof(GetById), new { id = part.Id }, new StockOperationResponse
            {
                Part   = _mapper.Map<SparePartResponse>(part),
                Alerts = alerts
            });
        }
        catch (InvalidOperationException ex) { return Conflict(new { message = ex.Message }); }
        catch (ArgumentException ex)         { return BadRequest(new { message = ex.Message }); }
    }

    /// <summary>Agregar stock — Strategy: AddStockStrategy (Administrador).</summary>
    [HttpPatch("{id:guid}/stock/add")]
    [Authorize(Roles = "Administrador")]
    [ProducesResponseType(typeof(StockOperationResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AddStock(Guid id, [FromBody] UpdateStockRequest request)
    {
        try
        {
            var (part, alerts) = await _sparePartService.AddStockAsync(id, request.Amount);
            return Ok(new StockOperationResponse
            {
                Part   = _mapper.Map<SparePartResponse>(part),
                Alerts = alerts
            });
        }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (ArgumentException ex)    { return BadRequest(new { message = ex.Message }); }
    }

    /// <summary>Retirar stock — Strategy: WithdrawStockStrategy (Administrador / Mecánico).</summary>
    [HttpPatch("{id:guid}/stock/withdraw")]
    [Authorize(Roles = "Administrador,Mecanico")]
    [ProducesResponseType(typeof(StockOperationResponse), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> WithdrawStock(Guid id, [FromBody] UpdateStockRequest request)
    {
        try
        {
            var (part, alerts) = await _sparePartService.WithdrawStockAsync(id, request.Amount);
            return Ok(new StockOperationResponse
            {
                Part   = _mapper.Map<SparePartResponse>(part),
                Alerts = alerts
            });
        }
        catch (KeyNotFoundException ex)    { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
        catch (ArgumentException ex)         { return BadRequest(new { message = ex.Message }); }
    }

    /// <summary>Eliminar repuesto (Administrador) — deshabilitado por regla de negocio; retorna 405.</summary>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Administrador")]
    public IActionResult Delete(Guid id) =>
        StatusCode(405, new { message = "Los repuestos no pueden eliminarse. Use actualización de stock." });
}
