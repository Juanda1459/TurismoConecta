namespace TurismoConecta.api.DTOs.Auth
{
    public class ResetPasswordRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string NuevaPassword { get; set; } = string.Empty;


    }
}
