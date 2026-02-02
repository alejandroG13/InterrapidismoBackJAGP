using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppiUni.Models
{

    public class Inscripciones
    {
        [Key]
        [Column("id_inscripcion")]
        public int IdInscripcion { get; set; }

        [Column("id_estudiante")]
        public int IdEstudiante { get; set; }
        public Estudiante Estudiante { get; set; } = null!;

        [Column("id_materia")]
        public int IdMateria { get; set; }
        public Materia Materia { get; set; } = null!;

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Column("fecha_actualizacion")]
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;
    }
}