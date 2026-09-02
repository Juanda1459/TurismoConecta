using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using TurismoConecta.api.Data;
using TurismoConecta.api.DTOs.Usuarios;
using TurismoConecta.api.Services.Interfaces;

namespace TurismoConecta.api.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env; 

        
        public UsuarioService(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
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

        
        public async Task<PerfilResponseDto?> ObtenerPerfilAsync(int idUsuario)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);

            if (usuario == null)
                return null;

            return new PerfilResponseDto
            {
                IdUsuario = usuario.IdUsuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Telefono = usuario.Telefono,
                Rol = usuario.IdRolNavigation.Nombre,
                FotoUrl = usuario.FotoUrl 
            };
        }

        
        public async Task<bool> ActualizarPerfilAsync(int idUsuario, ActualizarPerfilRequestDto dto)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario);

            if (usuario == null)
                return false;

            
            usuario.Nombre = dto.Nombre;
            usuario.Apellido = dto.Apellido;
            usuario.Telefono = dto.Telefono;

            
            if (!string.IsNullOrEmpty(dto.FotoBase64))
            {
                try
                {
                   
                    var base64Data = dto.FotoBase64;
                    if (base64Data.Contains(","))
                    {
                        base64Data = base64Data.Split(',')[1];
                    }

                    
                    byte[] imageBytes = Convert.FromBase64String(base64Data);

                    
                    var folderPath = Path.Combine(_env.WebRootPath, "images", "perfiles");
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    
                    var fileName = $"perfil_{usuario.IdUsuario}_{DateTime.UtcNow.Ticks}.jpg";
                    var filePath = Path.Combine(folderPath, fileName);

                    
                    if (!string.IsNullOrEmpty(usuario.FotoUrl))
                    {
                        var oldPath = Path.Combine(_env.WebRootPath, usuario.FotoUrl.TrimStart('/'));
                        if (File.Exists(oldPath)) File.Delete(oldPath);
                    }

                    
                    await File.WriteAllBytesAsync(filePath, imageBytes);

                    
                    usuario.FotoUrl = $"/images/perfiles/{fileName}";
                }
                catch (Exception)
                {
                    
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}