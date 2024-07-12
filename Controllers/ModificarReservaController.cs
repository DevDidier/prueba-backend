using dto.Models;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using prueba_backend.Models.Services;
using System.Diagnostics;

[ApiController]
[Route("modificar_reserva")]
public class ModificarReservaController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    public ModificarReservaController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpPatch]
    public IActionResult Patch([FromBody] ModifyReservaRequest request)
    {
        try
        {
            if (request == null || request.idReserva == 0
                || request.fechaini == default(DateTime) || request.fechafin == default(DateTime))
            {
                return StatusCode(404, new { code = 120, msm = "Datos vacios" });
            }

            var response = _usuarioService.ModificarReserva(request.idReserva, request.fechaini, request.fechafin);

            dynamic dynamicResponse = response;
            var status = (int)dynamicResponse.status;
            var code = (int)dynamicResponse.code;
            var msm = (string)dynamicResponse.msm;

            return StatusCode(status, new { code, msm });

        }
        catch (Exception error)
        {
            Debug.WriteLine($"Error controlador Login: {error}");
            return StatusCode(500, new { code = 130, msm = "Hubo un error" });
        }
    }
}