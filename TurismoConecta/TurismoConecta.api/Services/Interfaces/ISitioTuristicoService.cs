using TurismoConecta.api.DTOs.SitiosTuristicos;

namespace TurismoConecta.api.Services.Interfaces
{
    public interface ISitioTuristicoService
    {
        Task<List<SitioTuristicoDto>> ListarAsync(bool soloDestacados, int? idCategoria, CancellationToken ct = default);

        Task<(bool exito, int id, string? error)> CrearAsync(SitioTuristicoCrearDto dto, int idUsuarioSolicitante, CancellationToken ct = default);
        Task<(bool exito, string? error)> EditarAsync(int id, SitioTuristicoEditarDto dto, int idUsuarioSolicitante, CancellationToken ct = default);
        Task<(bool exito, string? error)> EliminarAsync(int id, int idUsuarioSolicitante, CancellationToken ct = default);
    }
}
