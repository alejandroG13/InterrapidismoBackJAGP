namespace WebAppiUni.DTOs.Estudiante
{
    public class EstudReadDto
    {
        public int IdEstudiante { get; set; }
        public required string Nombres { get; set; }
        public required string Apellidos { get; set; }
        public required string Email { get; set; }
    }
}
