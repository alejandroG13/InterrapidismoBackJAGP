using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppiUni.Data;
using WebAppiUni.DTOs;
using WebAppiUni.DTOs.Estudiante;
using WebAppiUni.Models;

namespace UniversidadAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class EstudiantesController : ControllerBase
    {
        private readonly UniversidadContext _context;
        private readonly IPasswordHasher<Estudiante> _passwordHasher;

        public EstudiantesController(UniversidadContext context, IPasswordHasher<Estudiante> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        // Obtener todos los Estudiantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EstudReadDto>>> Get()
        {
            var estudiante = await _context.Estudiantes.Select(e => new EstudReadDto
            {
                IdEstudiante = e.IdEstudiante,
                Nombres = e.Nombres,
                Apellidos = e.Apellidos,
                Email = e.Email
            }).ToListAsync(); 

            return Ok(estudiante);
        }

        // Obtener Estudiante por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<EstudReadDto>> Get(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);

            if (estudiante == null)
                return NotFound();

            return Ok(new EstudReadDto
            {
                IdEstudiante = estudiante.IdEstudiante,
                Nombres = estudiante.Nombres,
                Apellidos = estudiante.Apellidos,
                Email = estudiante.Email
            });
        }

        [AllowAnonymous]
        // Crear Estudiante
        [HttpPost]
        public async Task<IActionResult> Post(EstuCreateDto dto)
        {
            var estudiante = new Estudiante
            {
                Nombres = dto.Nombres,
                Apellidos = dto.Apellidos,
                Email = dto.Email,
                Password = string.Empty
            };

            estudiante.Password = _passwordHasher.HashPassword(estudiante, dto.Password);

            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = estudiante.IdEstudiante }, null);
        }

        // Actualizar Estudiante
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, EstuUpdateDto dto)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);

            if (estudiante == null)
                return NotFound();

            estudiante.Nombres = dto.Nombres;
            estudiante.Apellidos = dto.Apellidos;
            estudiante.Email = dto.Email;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Eliminar Estudiante
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);

            if (estudiante == null)
                return NotFound();

            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
