using dto.Models;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using prueba_backend.Models.Services;
using System.Diagnostics;

[ApiController]
[Route("misreservas")]
public class VerReservasController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public VerReservasController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpGet("{idUser}")]
    public IActionResult Get(int idUser)
    {
        try
        {
            var response = _usuarioService.VerReservas(idUser);

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