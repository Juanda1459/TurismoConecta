using TurismoConecta.api.DTOs.Municipios;
using TurismoConecta.api.Models;

namespace TurismoConecta.api.Services.Mappers
{
    public static class MunicipioMapper
    {
        public static MunicipioListadoDto ToListadoDto(Municipio m) => new()
        {
            IdMunicipio = m.IdMunicipio,
            Nombre = m.Nombre,
            ImagenUrl = m.ImagenUrl,
            Etiquetas = m.MunicipioEtiqueta.Select(me => me.IdEtiquetaNavigation.Nombre).ToList()
        };

        public static MunicipioFichaDto ToFichaDto(Municipio m) => new()
        {
            IdMunicipio = m.IdMunicipio,
            Nombre = m.Nombre,
            ImagenUrl = m.ImagenUrl,
            Descripcion = m.Descripcion,
            Clima = m.Clima,
            Historia = m.Historia,
            FechasRelevantes = m.FechasRelevantes,
            Latitud = m.Latitud,
            Longitud = m.Longitud,
            Etiquetas = m.MunicipioEtiqueta.Select(me => me.IdEtiquetaNavigation.Nombre).ToList()
        };
    }
}