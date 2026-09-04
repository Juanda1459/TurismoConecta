using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurismoConecta.api.DTOs.Common;
using TurismoConecta.api.DTOs.Municipios;
using TurismoConecta.api.Services.Interfaces;

namespace TurismoConecta.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MunicipiosController : ControllerBase
    {
        private readonly IMunicipioService _municipioService;
        public MunicipiosController(IMunicipioService municipioService) => _municipioService = municipioService;

        /// <summary>Lista municipios activos, paginados.</summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ResultadoPaginado<MunicipioListadoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Listar([FromQuery] int pagina = 1, [FromQuery] int tamano = 10, CancellationToken ct = default) =>
            Ok(await _municipioService.ListarAsync(pagina, tamano, ct));

        /// <summary>Busca municipios por texto y/o etiqueta.</summary>
        [HttpGet("buscar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(List<MunicipioListadoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Buscar([FromQuery] string? texto, [FromQuery] int? idEtiqueta, CancellationToken ct = default) =>
            Ok(await _municipioService.BuscarAsync(texto, idEtiqueta, ct));

        /// <summary>Ficha detallada de un municipio.</summary>
        [HttpGet("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(MunicipioFichaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Ficha(int id, CancellationToken ct = default)
        {
            var ficha = await _municipioService.ObtenerFichaAsync(id, ct);
            return ficha is null ? NotFound(new { mensaje = "Municipio no encontrado." }) : Ok(ficha);
        }

        /// <summary>Edita la ficha de un municipio. Solo el AdminMunicipal asignado a ese municipio.</summary>
        [HttpPut("{id:int}")]
        [Authorize(Roles = "AdminMunicipal")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Editar(int id, [FromBody] MunicipioEditarDto dto, CancellationToken ct = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var (exito, error) = await _municipioService.EditarAsync(id, idUsuario, dto, ct);

            if (exito) return NoContent();
            return error!.Contains("permiso") ? Forbid() : BadRequest(new { mensaje = error });
        }
    }
}