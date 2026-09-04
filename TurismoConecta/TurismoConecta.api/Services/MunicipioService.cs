using Microsoft.EntityFrameworkCore;
using TurismoConecta.api.Data;
using TurismoConecta.api.DTOs.Common;
using TurismoConecta.api.DTOs.Municipios;
using TurismoConecta.api.Services.Interfaces;
using TurismoConecta.api.Services.Mappers;

namespace TurismoConecta.api.Services
{
    public class MunicipioService : IMunicipioService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MunicipioService> _logger;

        public MunicipioService(AppDbContext context, ILogger<MunicipioService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResultadoPaginado<MunicipioListadoDto>> ListarAsync(int pagina, int tamano, CancellationToken ct = default)
        {
            // Clamp: nunca dejamos que el cliente pida página 0/negativa o un tamaño absurdo (protección básica contra abuso del endpoint)
            pagina = Math.Max(1, pagina);
            tamano = Math.Clamp(tamano, 1, 50);

            var query = _context.Municipios.Where(m => m.Activo);
            var total = await query.CountAsync(ct);

            var items = await query
                .OrderBy(m => m.Nombre)
                .Skip((pagina - 1) * tamano)
                .Take(tamano)
                .Include(m => m.MunicipioEtiqueta).ThenInclude(me => me.IdEtiquetaNavigation)
                .ToListAsync(ct);

            return new ResultadoPaginado<MunicipioListadoDto>
            {
                Items = items.Select(MunicipioMapper.ToListadoDto).ToList(),
                Total = total,
                Pagina = pagina,
                Tamano = tamano
            };
        }

        public async Task<List<MunicipioListadoDto>> BuscarAsync(string? texto, int? idEtiqueta, CancellationToken ct = default)
        {
            var query = _context.Municipios.Where(m => m.Activo).AsQueryable();

            if (!string.IsNullOrWhiteSpace(texto))
                query = query.Where(m => m.Nombre.Contains(texto.Trim()));

            if (idEtiqueta.HasValue)
                query = query.Where(m => m.MunicipioEtiqueta.Any(me => me.IdEtiqueta == idEtiqueta));

            var municipios = await query
                .Include(m => m.MunicipioEtiqueta).ThenInclude(me => me.IdEtiquetaNavigation)
                .ToListAsync(ct);

            return municipios.Select(MunicipioMapper.ToListadoDto).ToList();
        }

        public async Task<MunicipioFichaDto?> ObtenerFichaAsync(int id, CancellationToken ct = default)
        {
            var m = await _context.Municipios
                .Include(x => x.MunicipioEtiqueta).ThenInclude(me => me.IdEtiquetaNavigation)
                .FirstOrDefaultAsync(x => x.IdMunicipio == id && x.Activo, ct);

            return m is null ? null : MunicipioMapper.ToFichaDto(m);
        }

        public async Task<(bool exito, string? error)> EditarAsync(int id, int idAdminSolicitante, MunicipioEditarDto dto, CancellationToken ct = default)
        {
            var municipio = await _context.Municipios.FindAsync(new object?[] { id }, ct);
            if (municipio is null) return (false, "Municipio no encontrado.");

            var admin = await _context.Usuarios.FindAsync(new object?[] { idAdminSolicitante }, ct);
            if (admin is null || admin.MunicipioAsignadoId != id)
            {
                _logger.LogWarning("Usuario {IdUsuario} intentó editar el municipio {IdMunicipio} sin pertenecerle", idAdminSolicitante, id);
                return (false, "No tienes permiso para editar este municipio.");
            }

            municipio.Nombre = dto.Nombre;
            municipio.Descripcion = dto.Descripcion;
            municipio.Clima = dto.Clima;
            municipio.Historia = dto.Historia;
            municipio.FechasRelevantes = dto.FechasRelevantes;

            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Municipio {IdMunicipio} actualizado por el usuario {IdUsuario}", id, idAdminSolicitante);
            return (true, null);
        }
    }
}