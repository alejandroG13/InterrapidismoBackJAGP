using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppiUni.Data;
using WebAppiUni.DTOs.Materia;

namespace WebAppiUni.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MateriaController : Controller
    {
        private UniversidadContext _context;

        public MateriaController(UniversidadContext context)
        {
            _context = context;
        }

        //Obtener materias con profesor
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MateriaDetalleDto>>> Get()
        {
            var materia = await _context.Materias
                .Include(i => i.Profesor)
                .Select(i => new MateriaDetalleDto
                {
                    NombreMateria = i.Nombre,
                    NombreProfesor = i.Profesor.Nombre
                }).ToListAsync();

            return Ok(materia);
        }

    }
}
