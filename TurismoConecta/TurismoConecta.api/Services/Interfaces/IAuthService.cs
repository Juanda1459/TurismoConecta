using TurismoConecta.api.DTOs.Auth;
using System.Threading.Tasks;


namespace TurismoConecta.api.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegistrarAsync(RegisterRequestDto dto);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
    Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto dto);
    Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto);
}
