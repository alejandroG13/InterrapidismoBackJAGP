using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppiUni.Models
{
    public class Profesor
    {
        [Key]
        [Column("id_profesor")]
        public int IdProfesor { get; set; }

        [Column("nombre")]
        public required string Nombre { get; set; }

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Column("fecha_actualizacion")]
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        public ICollection<Materia> Materias { get; set; } = new List<Materia>();
    }
}
