using TurismoConecta.api.DTOs.Negocios;

namespace TurismoConecta.api.Services.Interfaces
{
    public interface INegocioService
    {
        Task<NegocioDto> CrearAsync(int idUsuario, NegocioCrearDto dto);
        Task<(bool exito, string? error)> EditarAsync(int idNegocio, int idUsuario, NegocioEditarDto dto);
        Task<(bool exito, string? error)> CambiarEstadoAsync(int idNegocio, int idAdminMunicipal, string nuevoEstado);
        Task<List<NegocioDto>> ListarPorMunicipioAsync(int idMunicipio, int? idCategoria);
        Task<List<NegocioPendienteDto>> ListarPendientesAsync(int idAdminMunicipal); // HU-24: bandeja
        Task<NegocioDto?> ObtenerFichaAsync(int idNegocio);
    }
}