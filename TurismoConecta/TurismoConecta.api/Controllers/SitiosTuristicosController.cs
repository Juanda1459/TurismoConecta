using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurismoConecta.api.DTOs.SitiosTuristicos;
using TurismoConecta.api.Services.Interfaces;
using TurismoConecta.api.Constants;

namespace TurismoConecta.api.Controllers
{
    [ApiController]
    [Route("api/sitios-turisticos")]
    public class SitiosTuristicosController : ControllerBase
    {
        private readonly ISitioTuristicoService _service;
        public SitiosTuristicosController(ISitioTuristicoService service) => _service = service;

        // GET api/sitios-turisticos
        // GET api/sitios-turisticos?soloDestacados=true
        // GET api/sitios-turisticos?idCategoria=5
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Listar(
            [FromQuery] bool soloDestacados = false,
            [FromQuery] int? idCategoria = null,
            CancellationToken ct = default)
            => Ok(await _service.ListarAsync(soloDestacados, idCategoria, ct));

        [HttpPost]
        [Authorize(Roles = Roles.AdminGeneral + "," + Roles.AdminMunicipio)]
        public async Task<IActionResult> Crear([FromBody] SitioTuristicoCrearDto dto, CancellationToken ct)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario is null) return Unauthorized();

            var (exito, id, error) = await _service.CrearAsync(dto, idUsuario.Value, ct);
            return exito ? Ok(new { idSitioTuristico = id }) : BadRequest(new { mensaje = error });
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.AdminGeneral + "," + Roles.AdminMunicipio)]
        public async Task<IActionResult> Editar(int id, [FromBody] SitioTuristicoEditarDto dto, CancellationToken ct)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario is null) return Unauthorized();

            var (exito, error) = await _service.EditarAsync(id, dto, idUsuario.Value, ct);
            return exito ? Ok() : BadRequest(new { mensaje = error });
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.AdminGeneral + "," + Roles.AdminMunicipio)]
        public async Task<IActionResult> Eliminar(int id, CancellationToken ct)
        {
            var idUsuario = ObtenerIdUsuario();
            if (idUsuario is null) return Unauthorized();

            var (exito, error) = await _service.EliminarAsync(id, idUsuario.Value, ct);
            return exito ? Ok() : BadRequest(new { mensaje = error });
        }

        private int? ObtenerIdUsuario()
            => int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;
    }
}
