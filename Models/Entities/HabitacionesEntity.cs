using prueba_backend.Models.Entities.Reservas;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prueba_backend.Models.Entities.habitaciones
{
    [Table("habitaciones")]
    public class HabitacionesEntity
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int habitacion {  get; set; }

        [Required]
        public int tipo { get; set; }
        
        public string ? imagen { get; set; }

        // Relación con ReservasEntity
        public virtual ICollection<ReservasEntity> Reservas { get; set; }
    }
}
