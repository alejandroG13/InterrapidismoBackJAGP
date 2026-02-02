using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAppiUni.Data;
using WebAppiUni.DTOs.Inscripciones;
using WebAppiUni.Models;

namespace WebAppiUni.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class InscripcionesController : ControllerBase
    {
        private readonly UniversidadContext _context;

        public InscripcionesController(UniversidadContext context)
        {
            _context = context;
        }

        // Crear inscripciones
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CrearInscripcion(InscCreateDto dto)
        {
            var idEstudianteClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (idEstudianteClaim == null)
                return Unauthorized(
                    new
                    {
                        message = "Token invalido"
                    });

            var idEstudiante = int.Parse(idEstudianteClaim.Value);

            // 2️⃣ Inscripciones actuales
            var inscripcionesActuales = await _context.Inscripciones
                .Where(i => i.IdEstudiante == idEstudiante)
                .Include(i => i.Materia)
                .ToListAsync();

            var cantidadActual = inscripcionesActuales.Count;

            if (cantidadActual >= 3)
                return BadRequest(
                    new
                    {
                        message = "El estudiante ya tiene el máximo de 3 materias"
                    });

            if (cantidadActual + dto.Materias.Count > 3)
                return BadRequest(
                    new
                    {
                        message = $"Solo puede inscribir {3 - cantidadActual} materia(s) más"
                    });

            // 3️⃣ Profesores actuales
            var profesoresActuales = inscripcionesActuales
                .Select(i => i.Materia.IdProfesor)
                .ToHashSet();

            // 4️⃣ Materias solicitadas
            var materias = await _context.Materias
                .Include(m => m.Profesor)
                .Where(m => dto.Materias.Contains(m.IdMateria))
                .ToListAsync();

            if (materias.Count != dto.Materias.Count)
                return BadRequest(new
                {
                    message = "Una o más materias no existen"
                });

            var nuevasInscripciones = new List<Inscripciones>();

            foreach (var materia in materias)
            {
                if (inscripcionesActuales.Any(i => i.IdMateria == materia.IdMateria))
                    return BadRequest(new
                    {
                        message = $"Ya está inscrito en {materia.Nombre}"
                    });

                if (profesoresActuales.Contains(materia.IdProfesor))
                    return BadRequest(new
                    {
                        message = $"No puede repetir profesor ({materia.Profesor.Nombre})"
                    });

                nuevasInscripciones.Add(new Inscripciones
                {
                    IdEstudiante = idEstudiante,
                    IdMateria = materia.IdMateria,
                    FechaRegistro = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                });

                profesoresActuales.Add(materia.IdProfesor);
            }

            _context.Inscripciones.AddRange(nuevasInscripciones);
            await _context.SaveChangesAsync();

            return Ok(
                new
                {
                    message = "Inscripciones creadas correctamente"
                });
        }


        // Obtener estudiantes inscritos por materia
        [HttpGet("materias_estudiantes")]
        public async Task<ActionResult<IEnumerable<InscDetalleDto>>>
        ObtenerMateriasConEstudiantes()
        {
            var resultado = await _context.Materias
                .Include(m => m.Profesor)
                .Include(m => m.Inscripciones)
                    .ThenInclude(i => i.Estudiante)
                .Select(m => new InscDetalleDto
                {
                    IdMateria = m.IdMateria,
                    NombreMateria = m.Nombre,
                    NombreProfesor = m.Profesor.Nombre,
                    EstudiantesInscritos = m.Inscripciones
                        .Select(i => $"{i.Estudiante.Nombres} {i.Estudiante.Apellidos}")
                        .ToList()
                })
                .ToListAsync();

            return Ok(resultado);
        }

        //Materias inscritas por usuario
        [HttpGet("materias_usuario")]
        [Authorize]
        public async Task<ActionResult<List<int>>> ObtenerMateriasInscritas()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var idEstudiante = int.Parse(userId!);

            var materias = await _context.Inscripciones
                .Where(i => i.IdEstudiante == idEstudiante)
                .Select(i => i.IdMateria)
                .ToListAsync();

            return Ok(materias);
        }
    }
}
