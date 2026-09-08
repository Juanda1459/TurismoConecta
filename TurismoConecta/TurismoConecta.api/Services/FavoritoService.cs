using Microsoft.EntityFrameworkCore;
using TurismoConecta.api.Data;
using TurismoConecta.api.DTOs.Favoritos;
using TurismoConecta.api.Models;
using TurismoConecta.api.Services.Interfaces;

namespace TurismoConecta.api.Services
{
    public class FavoritoService : IFavoritoService
    {
        private readonly AppDbContext _context;
        public FavoritoService(AppDbContext context) => _context = context;

        public async Task<(bool marcado, string? error)> ToggleAsync(int idUsuario, FavoritoToggleDto dto)
        {
            if (dto.IdMunicipio is null && dto.IdNegocio is null)
                return (false, "Debe indicar un municipio o un negocio.");

            var existente = await _context.Favoritos.FirstOrDefaultAsync(f =>
                f.IdUsuario == idUsuario && f.IdMunicipio == dto.IdMunicipio && f.IdNegocio == dto.IdNegocio);

            if (existente is not null)
            {
                _context.Favoritos.Remove(existente);
                await _context.SaveChangesAsync();
                return (false, null);
            }

            _context.Favoritos.Add(new Favorito
            {
                IdUsuario = idUsuario,
                IdMunicipio = dto.IdMunicipio,
                IdNegocio = dto.IdNegocio,
                FechaGuardado = DateTime.Now
            });
            await _context.SaveChangesAsync();
            return (true, null);
        }

        public async Task<List<FavoritoDto>> ListarTodosAsync(int idUsuario) =>
            await _context.Favoritos
                .Where(f => f.IdUsuario == idUsuario)
                .Include(f => f.IdMunicipioNavigation)
                .Include(f => f.IdNegocioNavigation)
                .Select(f => new FavoritoDto
                {
                    IdMunicipio = f.IdMunicipio,
                    NombreMunicipio = f.IdMunicipioNavigation != null ? f.IdMunicipioNavigation.Nombre : null,
                    IdNegocio = f.IdNegocio,
                    NombreNegocio = f.IdNegocioNavigation != null ? f.IdNegocioNavigation.Nombre : null,
                    FechaGuardado = f.FechaGuardado
                }).ToListAsync();
    }
}