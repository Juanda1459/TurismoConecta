using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurismoConecta.api.DTOs.SitiosTuristicos;
using TurismoConecta.api.Services.Interfaces;

namespace TurismoConecta.api.Controllers
{
    [ApiController]
    [Route("api/sitios-turisticos")]
    public class SitiosTuristicosController : ControllerBase
    {
        private readonly ISitioTuristicoService _service;
        public SitiosTuristicosController(ISitioTuristicoService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken ct)
            => Ok(await _service.ListarDestacadosAsync(ct));

        [HttpPost]
        [Authorize(Roles = "AdminGeneral,AdminMunicipio")]
        public async Task<IActionResult> Crear([FromBody] SitioTuristicoCrearDto dto, CancellationToken ct)
        {
            var idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var (exito, id, error) = await _service.CrearAsync(dto, idUsuario, ct);
            return exito ? Ok(new { idSitioTuristico = id }) : BadRequest(new { mensaje = error });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "AdminGeneral,AdminMunicipio")]
        public async Task<IActionResult> Editar(int id, [FromBody] SitioTuristicoEditarDto dto, CancellationToken ct)
        {
            var idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var (exito, error) = await _service.EditarAsync(id, dto, idUsuario, ct);
            return exito ? Ok() : BadRequest(new { mensaje = error });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "AdminGeneral,AdminMunicipio")]
        public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
        {
            var idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var (exito, error) = await _service.EliminarAsync(id, idUsuario, ct);
            return exito ? Ok() : BadRequest(new { mensaje = error });
        }
    }
}