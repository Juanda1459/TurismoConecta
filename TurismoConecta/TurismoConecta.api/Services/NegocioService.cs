using Microsoft.EntityFrameworkCore;
using TurismoConecta.api.Data;
using TurismoConecta.api.DTOs.Negocios;
using TurismoConecta.api.Models;
using TurismoConecta.api.Services.Interfaces;
using TurismoConecta.api.Services.Mappers;

namespace TurismoConecta.api.Services
{
    public class NegocioService : INegocioService
    {
        private readonly AppDbContext _context;
        public NegocioService(AppDbContext context) => _context = context;

        public async Task<NegocioDto> CrearAsync(int idUsuario, NegocioCrearDto dto)
        {
            var negocio = new Negocio
            {
                IdUsuario = idUsuario,
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                IdCategoria = dto.IdCategoria,
                IdMunicipio = dto.IdMunicipio,
                Telefono = dto.Telefono,
                Horario = dto.Horario,
                Direccion = dto.Direccion,
                Latitud = dto.Latitud,
                Longitud = dto.Longitud,
                Estado = "Pendiente", // HU-22
                FechaRegistro = DateTime.Now
            };

            // HU-22: galería de fotos junto con el registro, en la misma transacción
            negocio.GaleriaNegocios = dto.ImagenesGaleria
                .Where(url => !string.IsNullOrWhiteSpace(url))
                .Select(url => new GaleriaNegocio { ImagenUrl = url })
                .ToList();

            _context.Negocios.Add(negocio);
            await _context.SaveChangesAsync();

            return await ObtenerFichaAsync(negocio.IdNegocio) ?? throw new InvalidOperationException("Error al crear el negocio.");
        }

        public async Task<(bool exito, string? error)> EditarAsync(int idNegocio, int idUsuario, NegocioEditarDto dto)
        {
            var negocio = await _context.Negocios.FindAsync(idNegocio);
            if (negocio is null || negocio.IdUsuario != idUsuario)
                return (false, "Negocio no encontrado o no te pertenece.");

            // HU-23 exacto: "un cambio sustancial (nombre, categoría) requiere nueva verificación"
            // — solo esos 2 campos disparan la regla; horario/teléfono/dirección se editan libremente.
            bool cambioSustancial = negocio.Nombre != dto.Nombre || negocio.IdCategoria != dto.IdCategoria;

            negocio.Nombre = dto.Nombre;
            negocio.Descripcion = dto.Descripcion;
            negocio.IdCategoria = dto.IdCategoria;
            negocio.Telefono = dto.Telefono;
            negocio.Horario = dto.Horario;
            negocio.Direccion = dto.Direccion;
            negocio.Latitud = dto.Latitud;
            negocio.Longitud = dto.Longitud;

            if (cambioSustancial) negocio.Estado = "Pendiente";

            await _context.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool exito, string? error)> CambiarEstadoAsync(int idNegocio, int idAdminMunicipio, string nuevoEstado)
        {
            if (nuevoEstado != "Aprobado" && nuevoEstado != "Rechazado")
                return (false, "Estado inválido. Use 'Aprobado' o 'Rechazado'.");

            var negocio = await _context.Negocios.FindAsync(idNegocio);
            if (negocio is null) return (false, "Negocio no encontrado.");

            var admin = await _context.Usuarios.FindAsync(idAdminMunicipio);
            if (admin is null || admin.MunicipioAsignadoId != negocio.IdMunicipio)
                return (false, "No tienes permiso sobre este municipio.");

            negocio.Estado = nuevoEstado;
            if (nuevoEstado == "Aprobado") negocio.FechaAprobacion = DateTime.Now;

            await _context.SaveChangesAsync();
            // TODO (E8/SignalR): disparar notificación al propietario cuando el Hub esté portado al proyecto real
            return (true, null);
        }

        public async Task<List<NegocioDto>> ListarPorMunicipioAsync(int idMunicipio, int? idCategoria)
        {
            var query = _context.Negocios
                .Include(n => n.GaleriaNegocios)
                .Where(n => n.IdMunicipio == idMunicipio && n.Estado == "Aprobado"); // HU-25

            if (idCategoria.HasValue)
                query = query.Where(n => n.IdCategoria == idCategoria); // HU-25: filtro por categoría

            var negocios = await query.ToListAsync();
            return negocios.Select(n => NegocioMapper.ToDto(n)).ToList();
        }

        public async Task<List<NegocioPendienteDto>> ListarPendientesAsync(int idAdminMunicipal)
        {
            var admin = await _context.Usuarios.FindAsync(idAdminMunicipal);
            if (admin?.MunicipioAsignadoId is null) return new List<NegocioPendienteDto>();

            var pendientes = await _context.Negocios
                .Include(n => n.IdUsuarioNavigation)
                .Where(n => n.IdMunicipio == admin.MunicipioAsignadoId && n.Estado == "Pendiente")
                .OrderBy(n => n.FechaRegistro)
                .ToListAsync();

            return pendientes.Select(NegocioMapper.ToPendienteDto).ToList();
        }

        public async Task<NegocioDto?> ObtenerFichaAsync(int idNegocio)
        {
            var n = await _context.Negocios
                .Include(x => x.GaleriaNegocios)
                .FirstOrDefaultAsync(x => x.IdNegocio == idNegocio);
            if (n is null) return null;

            var promedio = await _context.Reseñas
                .Where(r => r.IdNegocio == idNegocio && r.Moderada)
                .AverageAsync(r => (double?)r.Calificacion);

            return NegocioMapper.ToDto(n, promedio);
        }
    }
}