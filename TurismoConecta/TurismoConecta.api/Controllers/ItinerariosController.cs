
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurismoConecta.api.DTOs.Itinerarios;
using TurismoConecta.api.Services.Interfaces;


namespace TurismoConecta.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItinerariosController : ControllerBase
    {
        private readonly IItinerarioService _service;

        public ItinerariosController(IItinerarioService service) => _service = service;

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Listar()
        {
            var idUsuario = ObtenerIdUsuarioActual();
            return Ok(await _service.ListarPorUsuarioAsync(idUsuario));
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<IActionResult> ObtenerDetalle(int id)
        {
            var idUsuario = ObtenerIdUsuarioActual();
            var itinerario = await _service.ObtenerDetalleAsync(id, idUsuario);
            return itinerario is null ? NotFound() : Ok(itinerario);
        }

        [HttpGet("compartido/{codigo:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerCompartido(Guid codigo)
        {
            var itinerario = await _service.ObtenerPorCodigoCompartirAsync(codigo);
            return itinerario is null ? NotFound("El enlace no es válido o el itinerario ya no está disponible.") : Ok(itinerario);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Crear([FromBody] ItinerarioCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var idUsuario = ObtenerIdUsuarioActual();
            var (exito, error, itinerario) = await _service.CrearAsync(idUsuario, dto);

            if (!exito) return BadRequest(new { mensaje = error });

            return CreatedAtAction(nameof(ObtenerDetalle), new { id = itinerario!.IdItinerario }, itinerario);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Eliminar(int id)
        {
            var idUsuario = ObtenerIdUsuarioActual();
            var (exito, error) = await _service.EliminarAsync(id, idUsuario);

            if (!exito)
                return error!.Contains("permiso") ? Forbid() : NotFound(error);

            return NoContent();
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ItinerarioUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var idUsuario = ObtenerIdUsuarioActual();
            var (exito, error, itinerario) = await _service.ActualizarAsync(id, idUsuario, dto);

            if (!exito)
                return error!.Contains("permiso") ? Forbid() : NotFound(new { mensaje = error });

            return Ok(itinerario);
        }


        [HttpPost("{id:int}/compartir")]
        [Authorize]
        public async Task<IActionResult> ToggleCompartir(int id)
        {
            var idUsuario = ObtenerIdUsuarioActual();
            var (exito, error, compartido, codigoCompartir) = await _service.ToggleCompartirAsync(id, idUsuario);

            if (!exito)
                return error!.Contains("permiso") ? Forbid() : NotFound(new { mensaje = error });

            return Ok(new { compartido, codigoCompartir });
        }


        private int ObtenerIdUsuarioActual()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(idClaim!);
        }


    }
}