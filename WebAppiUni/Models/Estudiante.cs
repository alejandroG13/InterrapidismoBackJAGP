using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAppiUni.Models
{
    public class Estudiante
    {
        [Key]
        [Column("id_estudiante")]
        public int IdEstudiante { get; set; }

        [Required]
        public required string Nombres { get; set; }

        [Required]
        public required string Apellidos { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        [Column("fecha_actualizacion")]
        public DateTime FechaActualizacion { get; set; } = DateTime.UtcNow;

        public ICollection<Inscripciones> Inscripciones { get; set; } = new List<Inscripciones>();
    }
}