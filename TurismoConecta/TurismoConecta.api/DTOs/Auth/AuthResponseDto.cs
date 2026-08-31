namespace TurismoConecta.api.DTOs.Auth;
public class AuthResponseDto
{
    public string Token { get; set; } = null!;
    public System.DateTime Expira { get; set; }
    public int IdUsuario { get; set; }
    public string NombreCompleto { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Rol { get; set; } = null!;
}
