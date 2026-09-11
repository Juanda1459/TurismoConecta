using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurismoConecta.api.DTOs.Reseñas;
using TurismoConecta.api.Services.Interfaces;

namespace TurismoConecta.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReseñasController : ControllerBase
    {
        private readonly IReseñaService _reseñaService;
        public ReseñasController(IReseñaService reseñaService) => _reseñaService = reseñaService;

        private int UsuarioActual => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Crear([FromBody] CrearReseñaDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var (exito, error) = await _reseñaService.CrearAsync(UsuarioActual, dto);
            return exito ? NoContent() : BadRequest(new { mensaje = error });
        }

        [HttpPatch("{id:int}/responder")]
        [Authorize(Roles = "AdminComercio")]
        public async Task<IActionResult> Responder(int id, [FromBody] string respuesta)
        {
            var (exito, error) = await _reseñaService.ResponderAsync(id, UsuarioActual, respuesta);
            if (exito) return NoContent();
            return error!.Contains("permiso") ? Forbid() : BadRequest(new { mensaje = error });
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "AdminMunicipio")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var (exito, error) = await _reseñaService.EliminarAsync(id, UsuarioActual);
            if (exito) return NoContent();
            return error!.Contains("permiso") ? Forbid() : BadRequest(new { mensaje = error });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Listar([FromQuery] int? idMunicipio, [FromQuery] int? idNegocio) =>
            Ok(await _reseñaService.ListarAsync(idMunicipio, idNegocio));
    }
}