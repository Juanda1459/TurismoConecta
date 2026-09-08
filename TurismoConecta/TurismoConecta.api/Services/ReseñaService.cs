using Microsoft.EntityFrameworkCore;
using TurismoConecta.api.Data;
using TurismoConecta.api.DTOs.Reseñas;
using TurismoConecta.api.Models;
using TurismoConecta.api.Services.Interfaces;

namespace TurismoConecta.api.Services
{
    public class ReseñaService : IReseñaService
    {
        private readonly AppDbContext _context;
        private static readonly string[] PalabrasProhibidas = { "estafa", "viagra", "compra-ya", "http://", "https://" };

        public ReseñaService(AppDbContext context) => _context = context;

        public async Task<(bool exito, string? error)> CrearAsync(int idUsuario, CrearReseñaDto dto)
        {
            if (dto.IdMunicipio is null && dto.IdNegocio is null)
                return (false, "Debe indicar un municipio o un negocio.");

            bool yaExiste = await _context.Reseñas.AnyAsync(r =>
                r.IdUsuario == idUsuario && r.IdMunicipio == dto.IdMunicipio && r.IdNegocio == dto.IdNegocio);
            if (yaExiste) return (false, "Ya publicaste una reseña para este lugar."); // HU-28: una activa por lugar

            bool esSpam = PalabrasProhibidas.Any(p =>
                (dto.Comentario ?? "").Contains(p, StringComparison.OrdinalIgnoreCase));

            _context.Reseñas.Add(new Reseña
            {
                IdUsuario = idUsuario,
                IdMunicipio = dto.IdMunicipio,
                IdNegocio = dto.IdNegocio,
                Calificacion = (byte)dto.Calificacion,
                Comentario = dto.Comentario,
                FechaCreacion = DateTime.Now,
                Moderada = !esSpam // HU-30: si detecta palabra prohibida, queda oculta hasta revisión
            });

            await _context.SaveChangesAsync();
            return (true, null);
        }

        public async Task<(bool exito, string? error)> ResponderAsync(int idReseña, int idDuenioNegocio, string respuesta)
        {
            var reseña = await _context.Reseñas
                .Include(r => r.IdNegocioNavigation)
                .FirstOrDefaultAsync(r => r.IdReseña == idReseña);

            if (reseña is null || reseña.IdNegocioNavigation is null)
                return (false, "Reseña no encontrada o no corresponde a un negocio.");

            if (reseña.IdNegocioNavigation.IdUsuario != idDuenioNegocio)
                return (false, "No tienes permiso para responder esta reseña.");

            // HU-29: "una única respuesta pública por reseña" — si ya respondió, se bloquea
            if (!string.IsNullOrEmpty(reseña.Respuesta))
                return (false, "Esta reseña ya tiene una respuesta.");

            reseña.Respuesta = respuesta;
            reseña.FechaRespuesta = DateTime.Now;
            await _context.SaveChangesAsync();
            return (true, null);
        }

        public async Task<List<ReseñaDto>> ListarAsync(int? idMunicipio, int? idNegocio)
        {
            var query = _context.Reseñas.Where(r => r.Moderada).AsQueryable();
            if (idMunicipio.HasValue) query = query.Where(r => r.IdMunicipio == idMunicipio);
            if (idNegocio.HasValue) query = query.Where(r => r.IdNegocio == idNegocio);

            return await query
                .Include(r => r.IdUsuarioNavigation)
                .OrderByDescending(r => r.FechaCreacion)
                .Select(r => new ReseñaDto
                {
                    IdReseña = r.IdReseña,
                    IdUsuario = r.IdUsuario,
                    NombreUsuario = r.IdUsuarioNavigation.Nombre,
                    Calificacion = r.Calificacion,
                    Comentario = r.Comentario,
                    Respuesta = r.Respuesta,
                    FechaCreacion = r.FechaCreacion
                }).ToListAsync();
        }

        public async Task<(bool exito, string? error)> EliminarAsync(int idReseña, int idAdminMunicipal)
        {
            var reseña = await _context.Reseñas
                .Include(r => r.IdNegocioNavigation)
                .FirstOrDefaultAsync(r => r.IdReseña == idReseña);
            if (reseña is null) return (false, "Reseña no encontrada.");

            // La reseña pertenece a un municipio directo, o a un municipio a través de su negocio
            var idMunicipioDeLaResena = reseña.IdMunicipio ?? reseña.IdNegocioNavigation?.IdMunicipio;

            var admin = await _context.Usuarios.FindAsync(idAdminMunicipal);
            if (admin is null || admin.MunicipioAsignadoId != idMunicipioDeLaResena)
                return (false, "No tienes permiso sobre esta reseña.");

            _context.Reseñas.Remove(reseña);
            await _context.SaveChangesAsync();
            return (true, null);
        }
    }
}