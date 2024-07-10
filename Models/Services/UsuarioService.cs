using Microsoft.EntityFrameworkCore;
using aplicationdbcontext;
using BCrypt.Net;
using prueba_backend.Models.Entities.Usuarios;
using prueba_backend.Models.ServicesToken;
using System.Diagnostics;

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

                Debug.WriteLine("sin " + password);
                Debug.WriteLine("clave" + hashedPassword);

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
                Console.WriteLine($"user {user}");

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

        public object CrearReserva(int idUser, int idRoom)
        {
            try
            {

            }
            catch (Exception error)
            {
                Debug.WriteLine($"error service crear reserva {error}");
            }
        }
    }
}
