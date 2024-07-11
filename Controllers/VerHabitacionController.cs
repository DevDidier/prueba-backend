using dto.Models;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using prueba_backend.Models.Services;
using System.Diagnostics;

[ApiController]
[Route("[controller]")]
public class VerHabitacionController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public VerHabitacionController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet("{id}")]
    public IActionResult Get(int id)
    {
        try
        {
            var response = _usuarioService.VerHabitacion(id);

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
