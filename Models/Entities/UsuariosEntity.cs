using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prueba_backend.Models.Entities.Usuarios
{
    [Table("usuarios")]
    public class UsuariosEntity
    {
        [Key]
        public int id { get; set; }

        [Required]
        [MaxLength(50)]
        public string username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string password { get; set; } = string.Empty;

        [Required]
        public int estado { get; set; }

        [Required]
        public DateTime FechaSys { get; set; } = DateTime.Now;

        public UsuariosEntity()
        {
            username = string.Empty;
            password = string.Empty;
        }
    }
}