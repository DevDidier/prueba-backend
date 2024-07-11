using Microsoft.EntityFrameworkCore;
using prueba_backend.Models.Entities.habitaciones;
using prueba_backend.Models.Entities.Reservas;
using prueba_backend.Models.Entities.Usuarios;

namespace aplicationdbcontext
{ 
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<UsuariosEntity> Usuarios { get; set; }
        public DbSet<HabitacionesEntity> Habitaciones { get; set; }
        public DbSet<ReservasEntity> Reservas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuraciones de tus entidades (si es necesario)
            base.OnModelCreating(modelBuilder);
        }
    }
}
