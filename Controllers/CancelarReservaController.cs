using dto.Models;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using prueba_backend.Models.Services;
using System.Diagnostics;

[ApiController]
[Route("cancelar_reserva")]
public class CancelarReservaController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    public CancelarReservaController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    [HttpDelete]
    public IActionResult Delete([FromBody] CancelReservaRequest request)
    {
        try
        {
            if (request.idUser == 0 || request.idReserva == 0)
            {
                return StatusCode(404, new { code = 120, msm = "Datos vacios" });
            }

            var response = _usuarioService.CancelarReserva(request.idUser, request.idReserva);

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