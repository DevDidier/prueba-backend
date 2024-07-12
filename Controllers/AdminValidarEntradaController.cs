using dto.Models;
using System;
using Newtonsoft.Json;
using prueba_backend.Models.ServicesToken;
using Microsoft.AspNetCore.Mvc;
using prueba_backend.Models.ServicesAdmin;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

[Authorize]
[ApiController]
[Route("admin/validar_entrada")]
public class AdminValidarEntradaController : ControllerBase
{
    private readonly IUAdminService _adminService;
    private readonly TokenService _tokenService;
    public AdminValidarEntradaController(IUAdminService usuarioService, TokenService tokenService)
    {
        _adminService = usuarioService;
        _tokenService = tokenService;
    }

    [HttpPatch("{idReserva}")]
    public IActionResult Patch(int idReserva)
    {
        try
        {
            var token = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var validate = _tokenService.ValidateAdminToken(token);

            if (!validate)
            {
                return StatusCode(401, new { msm = "No tiene permisos" });
            }

            var response = _adminService.ValidarEntrada(idReserva);

            dynamic dynamicResponse = response;
            var status = (int)dynamicResponse.status;
            var code = (int)dynamicResponse.code;
            var msm = (string)dynamicResponse.msm;

            return StatusCode(status, new { code, msm });
        }
        catch (Exception error)
        {
            Debug.WriteLine($"Error controlador Ver reservas: {error}");
            return StatusCode(500, new { code = 130, msm = "Hubo un error" });
        }
    }
}