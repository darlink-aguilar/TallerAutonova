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
    public class AdministratorController : ControllerBase
    {
        private readonly IAdministratorService _adminService;
        private readonly IMapper _mapper;

        public AdministratorController(IAdministratorService adminService, IMapper mapper)
        {
            _adminService = adminService;
            _mapper = mapper;
        }

        // ========== CRUD ADMINISTRATOR ==========

        // GET: api/administrator
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var admins = await _adminService.GetAllAsync();
            var response = _mapper.Map<IEnumerable<AdministratorResponseDTO>>(admins);
            return Ok(response);
        }

        // GET: api/administrator/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var admin = await _adminService.GetByIdAsync(id);
            if (admin == null)
                return NotFound($"Administrador con ID {id} no encontrado");

            var response = _mapper.Map<AdministratorResponseDTO>(admin);
            return Ok(response);
        }

        // POST: api/administrator
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AdministratorRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var admin = _mapper.Map<Administrator>(request);
            var created = await _adminService.CreateAsync(admin);
            var response = _mapper.Map<AdministratorResponseDTO>(created);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, response);
        }

        // PUT: api/administrator/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] AdministratorRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existing = await _adminService.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Administrador con ID {id} no encontrado");

            _mapper.Map(request, existing);
            await _adminService.UpdateAsync(existing);
            return NoContent();
        }

        // DELETE: api/administrator/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _adminService.GetByIdAsync(id);
            if (existing == null)
                return NotFound($"Administrador con ID {id} no encontrado");

            await _adminService.DeleteAsync(id);
            return NoContent();
        }

        // ========== GESTIÓN DE USUARIOS (FACTORY METHOD) ==========

        // POST: api/administrator/user
        [HttpPost("user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var user = await _adminService.CreateUserAsync(
                    request.Login,
                    request.Password,
                    request.Name,
                    request.AdministratorId,
                    request.Role
                );

                var response = _mapper.Map<UserResponseDTO>(user);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/administrator/users
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _adminService.GetAllUsersAsync();
            var response = _mapper.Map<IEnumerable<UserResponseDTO>>(users);
            return Ok(response);
        }

        // GET: api/administrator/users/mechanics
        [HttpGet("users/mechanics")]
        public async Task<IActionResult> GetAllMechanics()
        {
            var mechanics = await _adminService.GetAllMechanicsAsync();
            var response = _mapper.Map<IEnumerable<UserResponseDTO>>(mechanics);
            return Ok(response);
        }

        // GET: api/administrator/users/receptionists
        [HttpGet("users/receptionists")]
        public async Task<IActionResult> GetAllReceptionists()
        {
            var receptionists = await _adminService.GetAllReceptionistsAsync();
            var response = _mapper.Map<IEnumerable<UserResponseDTO>>(receptionists);
            return Ok(response);
        }

        // GET: api/administrator/users/{id}
        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _adminService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound($"Usuario con ID {id} no encontrado");

            var response = _mapper.Map<UserResponseDTO>(user);
            return Ok(response);
        }

        // DELETE: api/administrator/users/{id}
        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var existing = await _adminService.GetUserByIdAsync(id);
            if (existing == null)
                return NotFound($"Usuario con ID {id} no encontrado");

            await _adminService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}