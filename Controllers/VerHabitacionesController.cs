using dto.Models;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using prueba_backend.Models.Services;
using System.Diagnostics;

[ApiController]
[Route("[controller]")]
public class VerHabitacionesController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public VerHabitacionesController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    // Capturar el parámetro idUser de la ruta
    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            var response = _usuarioService.VerHabitaciones();

            dynamic dynamicResponse = response;
            var status = (int)dynamicResponse.status;
            var code = (int)dynamicResponse.code;
            var msm = (string)dynamicResponse.msm;
            var data = dynamicResponse.data;

            return StatusCode(status, new { code, msm, data });
        }
        catch (Exception error)
        {
            Debug.WriteLine($"Error controlador Ver reservas: {error}");
            return StatusCode(500, new { code = 130, msm = "Hubo un error" });
        }
    }
}