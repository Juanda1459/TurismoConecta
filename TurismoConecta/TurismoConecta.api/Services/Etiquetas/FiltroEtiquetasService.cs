using TurismoConecta.api.Data;
using Microsoft.EntityFrameworkCore;
using TurismoConecta.api.DTOs.Etiquetas;
public interface IFiltroEtiquetasService
{
    Task<List<MunicipioFiltradoDto>> FiltrarMunicipiosAsync(List<int>? idsEtiquetas);
    Task<List<int>> ObtenerEtiquetasPopularesAsync(int cantidad = 5);
}

public class FiltroEtiquetasService : IFiltroEtiquetasService
{
    private readonly AppDbContext _db;
    public FiltroEtiquetasService(AppDbContext db) => _db = db;

    public async Task<List<int>> ObtenerEtiquetasPopularesAsync(int cantidad = 5)
    {
       
        return await _db.MunicipioEtiqueta
            .GroupBy(me => me.IdEtiqueta)
            .OrderByDescending(g => g.Count())
            .Take(cantidad)
            .Select(g => g.Key)
            .ToListAsync();
    }

    public async Task<List<MunicipioFiltradoDto>> FiltrarMunicipiosAsync(List<int>? idsEtiquetas)
    {
        
        var etiquetasAUsar = (idsEtiquetas is null || idsEtiquetas.Count == 0)
            ? await ObtenerEtiquetasPopularesAsync()
            : idsEtiquetas;

        var municipios = await _db.Municipios
            .Where(m => m.Activo && m.MunicipioEtiqueta.Any(me => etiquetasAUsar.Contains(me.IdEtiqueta)))
            .Select(m => new MunicipioFiltradoDto
            {
                IdMunicipio = m.IdMunicipio,
                Nombre = m.Nombre,
                ImagenUrl = m.ImagenUrl,
                Etiquetas = m.MunicipioEtiqueta.Select(me => me.IdEtiquetaNavigation.Nombre).ToList()
            })
            .ToListAsync();

        return municipios;
    }
}