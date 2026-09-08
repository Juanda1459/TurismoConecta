using TurismoConecta.api.DTOs.Favoritos;

namespace TurismoConecta.api.Services.Interfaces
{
    public interface IFavoritoService
    {
        Task<(bool marcado, string? error)> ToggleAsync(int idUsuario, FavoritoToggleDto dto);
        Task<List<FavoritoDto>> ListarTodosAsync(int idUsuario);
    }
}