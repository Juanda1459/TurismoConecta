using Microsoft.EntityFrameworkCore;
using TurismoConecta.api.Data;
using TurismoConecta.api.Models;
using TurismoConecta.api.DTOs.SitiosTuristicos;
using TurismoConecta.api.Services.Interfaces;
using TurismoConecta.api.Constants;

namespace TurismoConecta.api.Services
{
    public class SitioTuristicoService : ISitioTuristicoService
    {
        private readonly AppDbContext _context;

        public SitioTuristicoService(AppDbContext context)
        {
            _context = context;
        }

        // ─────────────────────────────────────────────
        // LISTAR (público: lo usa la página de inicio)
        // ─────────────────────────────────────────────
        public async Task<List<SitioTuristicoDto>> ListarAsync(bool soloDestacados, int? idCategoria, CancellationToken ct = default)
        {
            var query = _context.SitiosTuristicos
                .AsNoTracking()
                .Where(s => s.Activo && s.IdMunicipioNavigation.Activo);

            if (soloDestacados)
                query = query.Where(s => s.Destacado);

            if (idCategoria.HasValue)
                query = query.Where(s => s.IdCategoria == idCategoria);

            return await query
                .OrderBy(s => s.IdCategoriaNavigation!.Nombre)
                .ThenBy(s => s.Nombre)
                .Select(s => new SitioTuristicoDto
                {
                    IdSitioTuristico = s.IdSitioTuristico,
                    Nombre = s.Nombre,
                    Descripcion = s.Descripcion,
                    ImagenUrl = s.ImagenUrl,

                    IdMunicipio = s.IdMunicipio,
                    NombreMunicipio = s.IdMunicipioNavigation.Nombre,
                    Clima = s.IdMunicipioNavigation.Clima,

                    Altitud = s.Altitud,
                    Destacado = s.Destacado,

                    IdCategoria = s.IdCategoria,
                    NombreCategoria = s.IdCategoriaNavigation != null ? s.IdCategoriaNavigation.Nombre : null,
                    IconoCategoria = s.IdCategoriaNavigation != null ? s.IdCategoriaNavigation.Icono : null,

                    Calificacion = s.IdMunicipioNavigation.Reseñas
                                         .Where(r => r.Moderada)
                                         .Average(r => (double?)r.Calificacion)
                })
                .ToListAsync(ct);
        }

        // ─────────────────────────────────────────────
        // VALIDACIONES PRIVADAS
        // ─────────────────────────────────────────────
        private async Task<bool> PuedeGestionarMunicipioAsync(int idUsuario, int idMunicipio, CancellationToken ct)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(u => u.IdUsuario == idUsuario, ct);

            if (usuario is null) return false;
            if (usuario.IdRolNavigation?.Nombre == Roles.AdminGeneral) return true;
            return usuario.IdRolNavigation?.Nombre == Roles.AdminMunicipio && usuario.MunicipioAsignadoId == idMunicipio;
        }

        private async Task<bool> CategoriaValidaParaSitioAsync(int? idCategoria, CancellationToken ct)
        {
            if (!idCategoria.HasValue) return true;
            return await _context.Categoria.AnyAsync(c => c.IdCategoria == idCategoria && c.AplicaSitio, ct);
        }

        // ─────────────────────────────────────────────
        // CREAR
        // ─────────────────────────────────────────────
        public async Task<(bool exito, int id, string? error)> CrearAsync(SitioTuristicoCrearDto dto, int idUsuarioSolicitante, CancellationToken ct = default)
        {
            if (!await _context.Municipios.AnyAsync(m => m.IdMunicipio == dto.IdMunicipio && m.Activo, ct))
                return (false, 0, "El municipio no existe o está inactivo.");

            if (!await PuedeGestionarMunicipioAsync(idUsuarioSolicitante, dto.IdMunicipio, ct))
                return (false, 0, "No tienes permiso para agregar sitios turísticos a este municipio.");

            if (!await CategoriaValidaParaSitioAsync(dto.IdCategoria, ct))
                return (false, 0, "La categoría no es válida para sitios turísticos.");

            var sitio = new SitioTuristico
            {
                Nombre = dto.Nombre.Trim(),
                Descripcion = dto.Descripcion?.Trim(),
                ImagenUrl = dto.ImagenUrl?.Trim(),
                IdMunicipio = dto.IdMunicipio,
                IdCategoria = dto.IdCategoria,
                Altitud = dto.Altitud,
                Destacado = dto.Destacado,
                Activo = true
            };

            _context.SitiosTuristicos.Add(sitio);
            await _context.SaveChangesAsync(ct);
            return (true, sitio.IdSitioTuristico, null);
        }

        // ─────────────────────────────────────────────
        // EDITAR
        // ─────────────────────────────────────────────
        public async Task<(bool exito, string? error)> EditarAsync(int id, SitioTuristicoEditarDto dto, int idUsuarioSolicitante, CancellationToken ct = default)
        {
            var sitio = await _context.SitiosTuristicos.FindAsync(new object?[] { id }, ct);
            if (sitio is null) return (false, "Sitio turístico no encontrado.");

            if (!await PuedeGestionarMunicipioAsync(idUsuarioSolicitante, sitio.IdMunicipio, ct))
                return (false, "No tienes permiso para editar este sitio turístico.");

            if (!await CategoriaValidaParaSitioAsync(dto.IdCategoria, ct))
                return (false, "La categoría no es válida para sitios turísticos.");

            sitio.Nombre = dto.Nombre.Trim();
            sitio.Descripcion = dto.Descripcion?.Trim();
            sitio.ImagenUrl = dto.ImagenUrl?.Trim();
            sitio.IdCategoria = dto.IdCategoria;
            sitio.Altitud = dto.Altitud;
            sitio.Destacado = dto.Destacado;

            await _context.SaveChangesAsync(ct);
            return (true, null);
        }

        // ─────────────────────────────────────────────
        // ELIMINAR (borrado lógico)
        // ─────────────────────────────────────────────
        public async Task<(bool exito, string? error)> EliminarAsync(int id, int idUsuarioSolicitante, CancellationToken ct = default)
        {
            var sitio = await _context.SitiosTuristicos.FindAsync(new object?[] { id }, ct);
            if (sitio is null) return (false, "Sitio turístico no encontrado.");

            if (!await PuedeGestionarMunicipioAsync(idUsuarioSolicitante, sitio.IdMunicipio, ct))
                return (false, "No tienes permiso para eliminar este sitio turístico.");

            sitio.Activo = false;
            await _context.SaveChangesAsync(ct);
            return (true, null);
        }
    }
}
