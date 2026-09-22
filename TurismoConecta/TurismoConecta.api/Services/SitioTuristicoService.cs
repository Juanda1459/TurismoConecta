using Microsoft.EntityFrameworkCore;
using TurismoConecta.api.Data;
using TurismoConecta.api.Models;
using TurismoConecta.api.DTOs.SitiosTuristicos;
using TurismoConecta.api.Services.Interfaces;

namespace TurismoConecta.api.Services
{
    public class SitioTuristicoService : ISitioTuristicoService
    {
        private readonly AppDbContext _context;

        public SitioTuristicoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SitioTuristicoDto>> ListarDestacadosAsync(CancellationToken ct = default)
        {
            return await _context.SitiosTuristicos
                .Include(s => s.IdMunicipioNavigation)
                .Include(s => s.IdReseñaNavigation)
                .OrderBy(s => s.IdSitioTuristico)
                .Select(s => new SitioTuristicoDto
                {
                    IdSitioTuristico = s.IdSitioTuristico,
                    Nombre = s.Nombre,
                    Descripcion = s.Descripcion,
                    ImagenUrl = s.ImagenUrl,
                    IdMunicipio = s.IdMunicipio,
                    NombreMunicipio = s.IdMunicipioNavigation.Nombre,
                    Calificacion = s.IdReseñaNavigation != null ? (double?)s.IdReseñaNavigation.Calificacion : null
                })
                .ToListAsync(ct);
        }
        private async Task<bool> PuedeGestionarMunicipioAsync(int idUsuario, int idMunicipio, CancellationToken ct)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario, ct);

            if (usuario is null) return false;
            if (usuario.IdRolNavigation?.Nombre == "AdminGeneral") return true;
            return usuario.IdRolNavigation?.Nombre == "AdminMunicipio" && usuario.MunicipioAsignadoId == idMunicipio;
        }

        public async Task<(bool exito, int id, string? error)> CrearAsync(SitioTuristicoCrearDto dto, int idUsuarioSolicitante, CancellationToken ct = default)
        {
            if (!await PuedeGestionarMunicipioAsync(idUsuarioSolicitante, dto.IdMunicipio, ct))
                return (false, 0, "No tienes permiso para agregar sitios turísticos a este municipio.");

            var sitio = new SitioTuristico
            {
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion,
                ImagenUrl = dto.ImagenUrl,
                IdMunicipio = dto.IdMunicipio
            };
            _context.SitiosTuristicos.Add(sitio);
            await _context.SaveChangesAsync(ct);
            return (true, sitio.IdSitioTuristico, null);
        }

        public async Task<(bool exito, string? error)> EditarAsync(int id, SitioTuristicoEditarDto dto, int idUsuarioSolicitante, CancellationToken ct = default)
        {
            var sitio = await _context.SitiosTuristicos.FindAsync(new object?[] { id }, ct);
            if (sitio is null) return (false, "Sitio turístico no encontrado.");

            if (!await PuedeGestionarMunicipioAsync(idUsuarioSolicitante, sitio.IdMunicipio, ct))
                return (false, "No tienes permiso para editar este sitio turístico.");

            sitio.Nombre = dto.Nombre.Trim();
            sitio.Descripcion = dto.Descripcion;
            sitio.ImagenUrl = dto.ImagenUrl;
            await _context.SaveChangesAsync(ct);
            return (true, null);
        }

        public async Task<(bool exito, string? error)> EliminarAsync(int id, int idUsuarioSolicitante, CancellationToken ct = default)
        {
            var sitio = await _context.SitiosTuristicos.FindAsync(new object?[] { id }, ct);
            if (sitio is null) return (false, "Sitio turístico no encontrado.");

            if (!await PuedeGestionarMunicipioAsync(idUsuarioSolicitante, sitio.IdMunicipio, ct))
                return (false, "No tienes permiso para eliminar este sitio turístico.");

            _context.SitiosTuristicos.Remove(sitio);
            await _context.SaveChangesAsync(ct);
            return (true, null);
        }
    }
}