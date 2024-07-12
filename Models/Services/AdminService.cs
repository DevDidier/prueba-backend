using aplicationdbcontext;
using prueba_backend.Models.ServicesAdmin;
using prueba_backend.Models.ServicesToken;
using System.Diagnostics;

namespace prueba_backend.Models.ServicesAdmin
{
    public class AdminService : IUAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;

        public AdminService(ApplicationDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public object VerAllReservas()
        {
            try
            {
                var reservas = _context.Reservas
                                       .Where(r => r.estado == 0)
                                       .ToList();
                if (!reservas.Any())
                {
                    return new { status = 404, code = 110, msm = "No se encontraron reservas", data = new List<object>() };
                }

                var data = reservas.Select(r => new
                {
                    r.id,
                    r.id_user,
                    r.id_habitacion,
                    r.estado,
                    r.fecha_inicio,
                    r.fecha_fin,
                    r.fechasys
                }).ToList();

                return new { status = 200, code = 100, msm = "Ok", data };
            }
            catch (Exception error)
            {
                Debug.WriteLine($"error service ver reservas {error}");
                return new { status = 500, code = 130, msm = "Server Error", data = new List<object>() };
            }
        }

        public object CancelarReserva(int idReserva)
        {
            try
            {
                var reservaExistente = _context.Reservas.FirstOrDefault(r => r.id == idReserva);

                if (reservaExistente == null)
                {
                    return new { status = 404, code = 120, msm = "Reserva no encontrada para el usuario especificado" };
                }

                _context.Reservas.Remove(reservaExistente); // Eliminar la reserva
                _context.SaveChanges();

                return new { status = 200, code = 100, msm = "Reserva eliminada exitosamente" };
            }
            catch (Exception error)
            {
                Debug.WriteLine($"Error al eliminar reserva: {error}");
                return new { status = 500, code = 130, msm = "Error en el servidor al intentar eliminar la reserva" };
            }
        }

        public object ValidarEntrada(int idReserva)
        {
            try
            {
                var reservaExistente = _context.Reservas.FirstOrDefault(r => r.id == idReserva);

                if (reservaExistente == null)
                {
                    return new { status = 404, code = 120, msm = "Reserva no encontrada para el usuario especificado" };
                }

                reservaExistente.estado = 1;
                _context.SaveChanges();

                return new { status = 200, code = 100, msm = "Entrada Validada" };
            }
            catch (Exception error)
            {
                Debug.WriteLine($"Error al cambiar estado de reserva: {error}");
                return new { status = 500, code = 130, msm = "Error en el servidor al intentar cambiar estado de reserva" };
            }
        }
    }
}
