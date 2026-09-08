using TurismoConecta.api.DTOs.Negocios;
using TurismoConecta.api.Models;

namespace TurismoConecta.api.Services.Mappers
{
    public static class NegocioMapper
    {
        public static NegocioDto ToDto(Negocio n, double? promedio = null) => new()
        {
            IdNegocio = n.IdNegocio,
            Nombre = n.Nombre,
            Descripcion = n.Descripcion,
            Estado = n.Estado,
            IdCategoria = n.IdCategoria,
            IdMunicipio = n.IdMunicipio,
            Telefono = n.Telefono,
            Horario = n.Horario,
            Direccion = n.Direccion,
            Latitud = n.Latitud,
            Longitud = n.Longitud,
            PromedioCalificacion = promedio,
            Galeria = n.GaleriaNegocios.Select(g => g.ImagenUrl!).Where(u => u != null).ToList()
        };

        public static NegocioPendienteDto ToPendienteDto(Negocio n) => new()
        {
            IdNegocio = n.IdNegocio,
            Nombre = n.Nombre,
            IdCategoria = n.IdCategoria,
            FechaRegistro = n.FechaRegistro,
            NombrePropietario = n.IdUsuarioNavigation?.Nombre ?? ""
        };
    }
}