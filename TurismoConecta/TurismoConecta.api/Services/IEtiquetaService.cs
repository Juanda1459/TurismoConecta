namespace TurismoConecta.api.Services;
using TurismoConecta.api.Data;  
using TurismoConecta.api.DTOs;
using Microsoft.EntityFrameworkCore;
using TurismoConecta.api.Models;

public interface IEtiquetaService
{
    Task<List<EtiquetaDto>> ObtenerTodasAsync();
    Task<EtiquetaDto?> ObtenerPorIdAsync(int id);
    Task<EtiquetaDto> CrearAsync(EtiquetaCrearDto dto);
    Task<bool> ActualizarAsync(int id, EtiquetaActualizarDto dto);
    Task<(bool exito, string? error)> DesactivarAsync(int id);
    Task<(bool exito, string? error)> EliminarAsync(int id);
}

public class EtiquetaService : IEtiquetaService
{
    private readonly AppDbContext _db;

    
    public EtiquetaService(AppDbContext db) => _db = db;

    public async Task<List<EtiquetaDto>> ObtenerTodasAsync()
    {
       
        return await _db.Etiqueta
            .Select(e => new EtiquetaDto
            {
                IdEtiqueta = e.IdEtiqueta,
                Nombre = e.Nombre,
                Descripcion = e.Descripcion,
                Activo = e.Activo
            })
            .ToListAsync();
    }

    public async Task<EtiquetaDto?> ObtenerPorIdAsync(int id)
    {
        var e = await _db.Etiqueta.FindAsync(id);
        if (e is null) return null; // el "?" en el tipo de retorno avisa que puede no existir

        return new EtiquetaDto
        {
            IdEtiqueta = e.IdEtiqueta,
            Nombre = e.Nombre,
            Descripcion = e.Descripcion,
            Activo = e.Activo
        };
    }

    public async Task<EtiquetaDto> CrearAsync(EtiquetaCrearDto dto)
    {
        var nueva = new Etiqueta
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            Activo = true 
        };

        _db.Etiqueta.Add(nueva);   
        await _db.SaveChangesAsync(); 

       
        return new EtiquetaDto
        {
            IdEtiqueta = nueva.IdEtiqueta,
            Nombre = nueva.Nombre,
            Descripcion = nueva.Descripcion,
            Activo = nueva.Activo
        };
    }

    public async Task<bool> ActualizarAsync(int id, EtiquetaActualizarDto dto)
    {
        var etiqueta = await _db.Etiqueta.FindAsync(id);
        if (etiqueta is null) return false;

        etiqueta.Nombre = dto.Nombre;
        etiqueta.Descripcion = dto.Descripcion;
        
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<(bool exito, string? error)> DesactivarAsync(int id)
    {
        var etiqueta = await _db.Etiqueta.FindAsync(id);
        if (etiqueta is null) return (false, "La etiqueta no existe.");

        etiqueta.Activo = false; 
        await _db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool exito, string? error)> EliminarAsync(int id)
    {
        var etiqueta = await _db.Etiqueta.FindAsync(id);
        if (etiqueta is null) return (false, "La etiqueta no existe.");

        
        bool enUsoPorMunicipio = await _db.MunicipioEtiqueta.AnyAsync(me => me.IdEtiqueta == id);
        bool enUsoPorNegocio = await _db.NegocioEtiqueta.AnyAsync(ne => ne.IdEtiqueta == id);

        if (enUsoPorMunicipio || enUsoPorNegocio)
            return (false, "No se puede eliminar: la etiqueta está siendo usada por al menos un municipio o negocio.");

        _db.Etiqueta.Remove(etiqueta);
        await _db.SaveChangesAsync();
        return (true, null);
    }
}