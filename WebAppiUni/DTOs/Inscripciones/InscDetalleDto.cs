namespace WebAppiUni.DTOs.Inscripciones
{
    public class InscDetalleDto
    {
        public required int IdMateria{ get; set; }
        public required string NombreMateria { get; set; }
        public required string NombreProfesor { get; set; }
        public List<string> EstudiantesInscritos { get; set; } = new();
    }
}
