using Microsoft.EntityFrameworkCore;
using aplicationdbcontext;
using BCrypt.Net;
using prueba_backend.Models.Entities.Usuarios;
using prueba_backend.Models.ServicesToken;
using System.Diagnostics;
using prueba_backend.Models.Entities.Reservas;

namespace prueba_backend.Models.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;
        private readonly TokenService _tokenService;

        public UsuarioService(ApplicationDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public object ValidateUser(string username, string password)
        {
            try
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                var user = _context.Usuarios
                .FirstOrDefault(u => u.username == username);

                Debug.WriteLine($"user respuesta {user}");
                if (user != null)
                {
                    if (BCrypt.Net.BCrypt.Verify(password, user.password))
                    {
                        Debug.WriteLine("Autenticación exitosa");
                        string tokenstring = _tokenService.GenerateToken(user.username, user.id);

                        if (tokenstring == null)
                        {
                            return new { status = 500, code = 140, msm = "No se pudo generar el token" };
                        }

                        return new
                        {
                            status = 200,
                            code = 100,
                            msm = "Inicio de sesión exitoso",
                            token = tokenstring
                        };
                    }
                    else
                    {
                        return new { status = 404, code = 110, msm = "Contraseña Incorrecta" };
                    }
                }
                else
                {
                    Debug.WriteLine("Autenticación fallida");
                    return new { status = 400, code = 110, msm = "No se Encontro el usuario" };
                }
            } 
            catch (Exception error)
            {
                Debug.WriteLine($"Error en service usuario {error}");
                return new { status = 500, code = 130, msm = "Server Error" };
            }
        }

        public object CreateUser(string username, string password) 
        {
            try
            {
                var user = _context.Usuarios
               .FirstOrDefault(u => u.username == username);

                if (user == null)
                {
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                    // Crear una nueva entidad de usuario
                    var newUser = new UsuariosEntity
                    {
                        username = username,
                        password = hashedPassword, // password encriptado
                        estado = 0, // Valor por defecto
                        FechaSys = DateTime.Now
                    };

                    _context.Usuarios.Add(newUser);
                    int result = _context.SaveChanges();

                    if (result > 0)
                    {
                        return new { status = 200, code = 100, msm = "Usuario Creado Correctamente" };
                    }
                    else
                    {
                        return new { status = 500, code = 130, msm = "No se pudo ingresar" };
                    }
                } 
                else 
                {
                    return new { status = 400, code = 110, msm = "Este nombre de usuario ya existe" };
                }
            }
            catch (Exception error) {
                Debug.WriteLine($"Error en service crear user {error}");
                return new { status = 500, code = 130, msm = "Server Error" };
            }
        }

        public object CrearReserva(int idUser, int idRoom, DateTime fechaini, DateTime fechafin)
        {
            try
            {
                //validar si existe una reserva en esas fechas
                var reservaExistente = _context.Reservas
                .FirstOrDefault(r => r.id_habitacion == idRoom && r.estado == 0 &&
                                ((r.fecha_inicio <= fechaini && r.fecha_fin >= fechaini) ||
                                 (r.fecha_inicio <= fechafin && r.fecha_fin >= fechafin) ||
                                 (r.fecha_inicio >= fechaini && r.fecha_fin <= fechafin)));

                if (reservaExistente != null)
                {
                    return new { status = 400, code = 110, msm = "Ya existe una reserva en las fechas seleccionadas." };
                }

                // Crear la nueva reserva si no existe una reserva superpuesta
                var nuevaReserva = new ReservasEntity
                {
                    id_user = idUser,
                    id_habitacion = idRoom,
                    fecha_inicio = fechaini,
                    fecha_fin = fechafin,
                    estado = 0, // 0 activo && 1 innactivo
                    fechasys = DateTime.Now
                };

                _context.Reservas.Add(nuevaReserva);
                _context.SaveChanges();

                return new { status = 200, code = 100, msm = "Reserva Creada" };
            }
            catch (Exception error)
            {
                Debug.WriteLine($"error service crear reserva {error}");
                return new { status = 500, code = 130, msm = "Server Error" };
            }
        }

        public object VerReservas(int idUser)
        {
            try
            {
                // Traer todas las reservas que tengan id_user = idUser
                var reservas = _context.Reservas
                                       .Include(h => h.Habitacion)
                                       .Where(r => r.id_user == idUser && r.estado == 0)
                                       .ToList();
                if (!reservas.Any())
                {
                    return new { status = 404, code = 110, msm = "No se encontraron reservas", data = new List<object>() };
                }

                // Convertir las reservas a formato JSON
                var data = reservas.Select(r => new
                {
                    r.id,
                    r.id_user,
                    r.id_habitacion,
                    r.estado,
                    r.fecha_inicio,
                    r.fecha_fin,
                    r.fechasys,
                    habitacion = new
                    {
                        r.Habitacion.nombre,
                        r.Habitacion.habitacion
                    }
                }).ToList();

                return new { status = 200, code = 100, msm = "Ok", data };
            }
            catch (Exception error)
            {
                Debug.WriteLine($"error service ver reservas {error}");
                return new { status = 500, code = 130, msm = "Server Error", data = new List<object>() };
            }
        }

        public object ModificarReserva(int idReserva, DateTime nuevaFechaInicio, DateTime nuevaFechaFin)
        {
            try
            {
                var reservaExistente = _context.Reservas.FirstOrDefault(r => r.id == idReserva);

                if (reservaExistente == null)
                {
                    return new { status = 404, code = 120, msm = "Reserva no encontrada" };
                }

                var reservaEnNuevasFechas = _context.Reservas
                    .FirstOrDefault(r =>
                        r.id_habitacion == reservaExistente.id_habitacion &&
                        r.id != idReserva && // Excluir la reserva actual
                        r.estado == 0 && //     Reserva activa
                        (
                            (r.fecha_inicio <= nuevaFechaInicio && r.fecha_fin >= nuevaFechaInicio) ||
                            (r.fecha_inicio <= nuevaFechaFin && r.fecha_fin >= nuevaFechaFin) ||
                            (r.fecha_inicio >= nuevaFechaInicio && r.fecha_fin <= nuevaFechaFin)
                        ));

                if (reservaEnNuevasFechas != null)
                {
                    return new { status = 400, code = 110, msm = "Ya existe una reserva en las nuevas fechas seleccionadas." };
                }

                reservaExistente.fecha_inicio = nuevaFechaInicio;
                reservaExistente.fecha_fin = nuevaFechaFin;

                _context.SaveChanges();

                return new { status = 200, code = 100, msm = "Reserva modificada exitosamente" };
            }
            catch (Exception error)
            {
                Debug.WriteLine($"Error al modificar reserva: {error}");
                return new { status = 500, code = 130, msm = "Error en el servidor" };
            }
        }

        public object VerHabitaciones()
        {
            try
            {
                var habitaciones = _context.Habitaciones.ToList();
          
                var data = habitaciones.Select(h => new
                {
                    h.id,
                    h.habitacion,
                    h.imagen,
                    h.nombre,
                    h.valor,
                    h.tipo,
                }).ToList();

                return new { status = 200, code = 100, msm = "Ok", data };
            }
            catch (Exception error)
            {
                Debug.WriteLine($"error service ver reservas {error}");
                return new { status = 500, code = 130, msm = "Server Error", data = new List<object>() };
            }
        }

        public object VerHabitacion(int idRoom)
        {
            try
            {
                var hoy = DateTime.Now;
                var habitacion = _context.Habitaciones
                                         .Include(h => h.Reservas)
                                         .FirstOrDefault(h => h.id == idRoom);

                if (habitacion == null)
                {
                    return new { status = 404, code = 110, msm = "No se encontro la habitación", data = new { } };
                }

                var data = new
                {
                    habitacion.id,
                    habitacion.habitacion,
                    habitacion.imagen,
                    habitacion.nombre,
                    habitacion.valor,
                    habitacion.tipo,
                    habitacion.descripcion,
                    reservas = habitacion.Reservas.Where(r => r.estado == 0 && r.fecha_fin > hoy).Select(r => new
                    {
                        r.fecha_inicio,
                        r.fecha_fin,
                        r.estado,
                        r.id_user,
                        r.fechasys
                    }).ToList()
                };

                return new { status = 200, code = 100, msm = "Ok", data };
            }
            catch (Exception error)
            {
                Debug.WriteLine($"Error al obtener la habitacion service: {error}");
                return new { status = 500, code = 130, msm = "Server Error", data = new { } };
            }
        }

        public object CancelarReserva(int idUser, int idReserva)
        {
            try
            {
                var reservaExistente = _context.Reservas.FirstOrDefault(r => r.id == idReserva && r.id_user == idUser);

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
    }
}
