namespace dto.Models
{
    public class UsuarioRequest
    {
        public string ? Usuario { get; set; }
        public string ? Contrasena { get; set; }
    }

    public class CrearReservaRequest
    {
        public int idUser { get; set; }
        public int idRoom { get; set; }
        public DateTime fechaini { get; set; }
        public DateTime fechafin { get; set; }
    }

    public class ModifyReservaRequest
    {
        public int idReserva { get; set; }
        public DateTime fechaini { get; set; }
        public DateTime fechafin { get; set; }
    }
}