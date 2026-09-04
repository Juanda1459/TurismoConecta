using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TurismoConecta.api.DTOs.Usuarios;
using TurismoConecta.api.Services.Interfaces;

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

        [HttpPut("perfil")]
        [Authorize]
        public async Task<IActionResult> ActualizarPerfil([FromBody] ActualizarPerfilDto dto)
        {
            var idUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var actualizado = await _usuarioService.ActualizarPerfilAsync(idUsuario, dto); // aquí sí dice _usuarioService
            return actualizado is null ? NotFound() : Ok(actualizado);
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
        
    }
}