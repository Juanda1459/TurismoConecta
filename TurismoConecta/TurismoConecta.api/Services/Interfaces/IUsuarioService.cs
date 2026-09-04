using TurismoConecta.api.DTOs.Usuarios;

namespace TurismoConecta.api.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<bool> AsignarRolAsync(AssignRoleRequestDto dto);
        Task<PerfilResponseDto?> ObtenerPerfilAsync(int idUsuario);
        Task<bool> ActualizarPerfilAsync(int idUsuario, ActualizarPerfilRequestDto dto);

    }
}