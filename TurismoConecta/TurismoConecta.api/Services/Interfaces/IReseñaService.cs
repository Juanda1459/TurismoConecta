using TurismoConecta.api.DTOs.Reseñas;

namespace TurismoConecta.api.Services.Interfaces
{
    public interface IReseñaService
    {
        Task<(bool exito, string? error)> CrearAsync(int idUsuario, CrearReseñaDto dto);
        Task<(bool exito, string? error)> ResponderAsync(int idReseña, int idDuenioNegocio, string respuesta);
        Task<List<ReseñaDto>> ListarAsync(int? idMunicipio, int? idNegocio);

        // HU-30: el admin municipal elimina una reseña reportada por spam/ofensiva
        Task<(bool exito, string? error)> EliminarAsync(int idReseña, int idAdminMunicipal);
    }
}