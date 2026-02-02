using System.ComponentModel.DataAnnotations;

namespace WebAppiUni.DTOs
{
    public class EstuCreateDto
    {
        [Required]
        public required string Nombres { get; set; }

        [Required]
        public required string Apellidos { get; set; }

        [Required, EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }
    }
}
