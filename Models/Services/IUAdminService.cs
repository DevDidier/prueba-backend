namespace prueba_backend.Models.ServicesAdmin
{
    public interface IUAdminService
    {
        object VerAllReservas();
        object CancelarReserva(int idReserva);
        object ValidarEntrada(int idReserva);
    }
}
