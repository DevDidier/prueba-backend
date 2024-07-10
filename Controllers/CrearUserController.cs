using dto.Models;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using prueba_backend.Models.Services;

[ApiController]
[Route("[controller]")]
public class CrearUserController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    public CrearUserController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPost]
    public IActionResult Post([FromBody] UsuarioRequest request)
    {
        try
        {
            if (request == null || string.IsNullOrEmpty(request.Usuario) || string.IsNullOrEmpty(request.Contrasena))
            {
                return StatusCode(404, new { code = 120, msm = "Usuario o contraseña no pueden estar vacios" });
            }

            var response = _usuarioService.CreateUser(request.Usuario, request.Contrasena);

            dynamic dynamicResponse = response;
            var status = (int)dynamicResponse.status;
            var code = (int)dynamicResponse.code;
            var msm = (string)dynamicResponse.msm;

            return StatusCode(status, new { code, msm });

        }
        catch (Exception error)
        {
            Console.WriteLine($"Error controlador Login: {error}");
            return StatusCode(500, new { code = 130, msm = "Hubo un error" });
        }
    }
}

