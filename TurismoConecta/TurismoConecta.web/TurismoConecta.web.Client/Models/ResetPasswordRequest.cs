namespace TurismoConecta.web.Client.Models
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string NuevaPassword { get; set; } = string.Empty;

        // Agregamos esta propiedad solo para validar en el frontend 
        // que el usuario no se equivoque al escribir su nueva clave.
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}