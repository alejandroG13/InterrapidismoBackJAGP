using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppiUni.Models
{
    public class Materia
    {
        [Key]
        [Column("id_materia")]
        public int IdMateria { get; set; }

        [Column("nombre")]
        public required string Nombre { get; set; }

        [Column("creditos")]
        public int Creditos { get; set; } = 3;

        [Column("id_profesor")]
        public int IdProfesor { get; set; }
        public Profesor Profesor { get; set; } = null!;

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Column("fecha_actualizacion")]
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        public ICollection<Inscripciones> Inscripciones { get; set; } = new List<Inscripciones>();
    }
}