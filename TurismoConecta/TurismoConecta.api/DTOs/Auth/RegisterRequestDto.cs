namespace TurismoConecta.api.DTOs.Auth;
public class RegisterRequestDto
{
    public string Nombre { get; set; } = null!;
    public string Apellido { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? Telefono { get; set; }
}
