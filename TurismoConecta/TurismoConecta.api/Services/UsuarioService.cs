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
        public async Task<PerfilDto?> ActualizarPerfilAsync(int idUsuario, ActualizarPerfilDto dto)
        {
            var usuario = await _context.Usuarios.FindAsync(idUsuario);  
            if (usuario is null) return null;

            usuario.Nombre = dto.Nombre;
            usuario.Apellido = dto.Apellido;
            usuario.Telefono = dto.Telefono;
            await _context.SaveChangesAsync();   

            return new PerfilDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Telefono = usuario.Telefono
            };
        }
    }
}