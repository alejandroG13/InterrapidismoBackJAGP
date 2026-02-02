using Microsoft.EntityFrameworkCore;
using WebAppiUni.Models;

namespace WebAppiUni.Data
{
    public class UniversidadContext : DbContext
    {
        public UniversidadContext(DbContextOptions<UniversidadContext> options) : base(options)
        {
        }

        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<Profesor> Profesores { get; set; }
        public DbSet<Materia> Materias { get; set; }
        public DbSet<Inscripciones> Inscripciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /* Relaciones entre tablas */
            // Materia -> Profesor
            modelBuilder.Entity<Materia>()
                .HasOne(m => m.Profesor)
                .WithMany(p => p.Materias)
                .HasForeignKey(m => m.IdProfesor)
                .OnDelete(DeleteBehavior.Restrict);

            // Inscripciones -> Estudiante
            modelBuilder.Entity<Inscripciones>()
                .HasOne(i => i.Estudiante)
                .WithMany(e => e.Inscripciones)
                .HasForeignKey(i => i.IdEstudiante)
                .OnDelete(DeleteBehavior.Restrict);

            // Inscripciones -> Materia
            modelBuilder.Entity<Inscripciones>()
                .HasOne(i => i.Materia)
                .WithMany(m => m.Inscripciones)
                .HasForeignKey(i => i.IdMateria)
                .OnDelete(DeleteBehavior.Restrict);

            // Índice único (igual que la BD)
            modelBuilder.Entity<Inscripciones>()
                .HasIndex(i => new { i.IdEstudiante, i.IdMateria })
                .IsUnique();
        }
    }
}