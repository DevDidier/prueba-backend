using Microsoft.EntityFrameworkCore;
using aplicationdbcontext;

namespace prueba_backend.Models.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public object ValidateUser(string username, string password)
        {
            try
            {
                var user = _context.Usuarios
                .FirstOrDefault(u => u.username == username && u.password == password && u.estado == 0);

                Console.WriteLine($"user {user}");

                if (user == null)
                {
                    return new { status = 400, code = 110, msm = "No se Encontro el usuario y contraseña" };
                }

                return new { status = 200, code = 100, msm = "Inicio de sesión exitoso" };

            } catch (Exception error)
            {
                Console.WriteLine($"Error en service usuario {error}");
                return new { status = 500, code = 130, msm = "Server Error" };
            }
        }
    }
}
