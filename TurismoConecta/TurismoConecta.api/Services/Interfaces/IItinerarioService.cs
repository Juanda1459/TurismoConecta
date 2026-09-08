// Services/Interfaces/IItinerarioService.cs
using TurismoConecta.api.DTOs.Itinerarios;

namespace TurismoConecta.api.Services.Interfaces
{
    public interface IItinerarioService
    {
        Task<List<ItinerarioListadoDto>> ListarPorUsuarioAsync(int idUsuario);
        Task<ItinerarioResponseDto?> ObtenerDetalleAsync(int idItinerario, int idUsuario);
        Task<(bool exito, string? error, ItinerarioResponseDto? itinerario)> CrearAsync(int idUsuario, ItinerarioCreateDto dto);
        Task<(bool exito, string? error)> EliminarAsync(int idItinerario, int idUsuario);
        Task<ItinerarioResponseDto?> ObtenerPorCodigoCompartirAsync(Guid codigo);
        Task<(bool exito, string? error, ItinerarioResponseDto? itinerario)> ActualizarAsync(int idItinerario, int idUsuario, ItinerarioUpdateDto dto);
        Task<(bool exito, string? error, bool compartido, Guid codigoCompartir)> ToggleCompartirAsync(int idItinerario, int idUsuario);




    }

}