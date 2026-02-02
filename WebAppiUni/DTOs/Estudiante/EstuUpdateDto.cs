using System.ComponentModel.DataAnnotations;

namespace WebAppiUni.DTOs.Estudiante
{
    public class EstuUpdateDto
    {
        [Required]
        public required string Nombres { get; set; }

        [Required]
        public required string Apellidos { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }
    }
}
