namespace prueba_backend.Models.Services
{
    public interface IUsuarioService
    {
        object ValidateUser(string username, string password);
        object CreateUser(string username, string password);
        object CrearReserva(int idUser, int idRoom, DateTime fechaini, DateTime fechafin);
        object VerReservas(int idUser);
        object ModificarReserva(int idReserva, DateTime nuevaFechaInicio, DateTime nuevaFechaFin);
        object VerHabitaciones();
        object VerHabitacion(int idRoom);
        object CancelarReserva(int idUser, int idReservas);
    }
}