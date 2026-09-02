namespace TurismoConecta.api.DTOs.Usuarios
{
    public class AssignRoleRequestDto
    {
        public int IdUsuario { get; set; }
        public string NombreRol { get; set; } = string.Empty;
    }
}