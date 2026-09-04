
namespace TurismoConecta.api.DTOs.Usuarios
{
    public class ActualizarPerfilRequestDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? FotoBase64 { get; set; } 

    }
}