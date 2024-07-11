using prueba_backend.Models.Entities.habitaciones;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prueba_backend.Models.Entities.Reservas
{
    [Table("reservas")]
    public class ReservasEntity
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int id_user { get; set; }

        [Required]
        public int id_habitacion { get; set; }

        [Required]
        public int estado { get; set; }

        [Required]
        public DateTime fecha_inicio {  get; set; }

        [Required]
        public DateTime fecha_fin  { get; set; }

        [Required]
        public DateTime fechasys { get; set; } = DateTime.Now;

        // Relación con HabitacionesEntity
        [ForeignKey("id_habitacion")]
        public virtual HabitacionesEntity Habitacion { get; set; }
    }
}
