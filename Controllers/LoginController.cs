using dto.Models;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using prueba_backend.Models.Services;
using System.Diagnostics;

[ApiController]
[Route("login")]

public class LoginController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    public LoginController(IUsuarioService usuarioService)
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

            var response = _usuarioService.ValidateUser(request.Usuario, request.Contrasena);

            dynamic dynamicResponse = response;
            var status = (int)dynamicResponse.status;
            var code = (int)dynamicResponse.code;
            var msm = (string)dynamicResponse.msm; 
            
            string token = "";

            var tokenProperty = response.GetType().GetProperty("token");
            if (tokenProperty != null)
            {
                token = (string)dynamicResponse.token;
            }

            return StatusCode(status, new { code, msm, token });

        }
        catch (Exception error)
        {
            Debug.WriteLine($"Error controlador Login: {error}");
            return StatusCode(500, new { code = 130, msm = "Hubo un error" });
        }
    }
}

