using Microsoft.EntityFrameworkCore;
using TurismoConecta.api.Data;
using TurismoConecta.api.DTOs.Usuarios;
using TurismoConecta.api.Services.Interfaces;

namespace TurismoConecta.api.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AsignarRolAsync(AssignRoleRequestDto dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.IdUsuario == dto.IdUsuario);

            if (usuario == null)
                return false;

            var rolNuevo = await _context.Rols
                .FirstOrDefaultAsync(r => r.Nombre == dto.NombreRol);

            if (rolNuevo == null)
                return false;

            usuario.IdRol = rolNuevo.IdRol;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}