using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TurismoConecta.api.DTOs.Usuarios;
using TurismoConecta.api.Services.Interfaces;
using System.Security.Claims; 

namespace TurismoConecta.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPut("asignar-rol")]
        [Authorize(Roles = "AdminGeneral")]
        public async Task<IActionResult> AsignarRol(AssignRoleRequestDto dto)
        {
            var exito = await _usuarioService.AsignarRolAsync(dto);

            if (!exito)
                return BadRequest("No se pudo asignar el rol. Verifica el idUsuario y el nombre del rol.");

            return Ok("Rol asignado correctamente.");
        }

        [HttpGet("perfil")]
        [Authorize]
        public async Task<IActionResult> ObtenerPerfil()
        {
            var idUsuario = ObtenerIdUsuarioActual();
            var perfil = await _usuarioService.ObtenerPerfilAsync(idUsuario);

            if (perfil == null)
                return NotFound("No se encontró el usuario.");

            return Ok(perfil);
        }

        [HttpPut("perfil")]
        [Authorize]
        public async Task<IActionResult> ActualizarPerfil(ActualizarPerfilRequestDto dto)
        {
            var idUsuario = ObtenerIdUsuarioActual();
            var exito = await _usuarioService.ActualizarPerfilAsync(idUsuario, dto);

            if (!exito)
                return NotFound("No se encontró el usuario.");

            return Ok("Perfil actualizado correctamente.");
        }

        private int ObtenerIdUsuarioActual()
        {
            var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(idClaim!);
        }


    }
}