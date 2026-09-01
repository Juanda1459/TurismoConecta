using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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