using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurismoConecta.api.DTOs.Favoritos;
using TurismoConecta.api.Services.Interfaces;

namespace TurismoConecta.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FavoritosController : ControllerBase
    {
        private readonly IFavoritoService _favoritoService;
        public FavoritosController(IFavoritoService favoritoService) => _favoritoService = favoritoService;

        private int UsuarioActual => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        [HttpPost("toggle")]
        public async Task<IActionResult> Toggle([FromBody] FavoritoToggleDto dto)
        {
            var (marcado, error) = await _favoritoService.ToggleAsync(UsuarioActual, dto);
            if (error is not null) return BadRequest(new { mensaje = error });
            return Ok(new { marcado });
        }

        // HU-27: lista completa (municipios + negocios) para la pantalla de perfil
        [HttpGet("mios")]
        public async Task<IActionResult> Mios() => Ok(await _favoritoService.ListarTodosAsync(UsuarioActual));
    }
}