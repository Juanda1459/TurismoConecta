namespace TurismoConecta.api.DTOs.Usuarios
{
    public class UsuarioAdminDto
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string Rol { get; set; } = string.Empty;
        public int? MunicipioAsignadoId { get; set; }
        public string? NombreMunicipioAsignado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
        public string? FotoUrl { get; set; }
    }

    public class RolDto
    {
        public int IdRol { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    public class UsuarioAdminEdicionDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string Rol { get; set; } = string.Empty;
        public int? MunicipioAsignadoId { get; set; }
        public bool Activo { get; set; } = true;
        public string? FotoBase64 { get; set; }
        public bool EliminarFoto { get; set; }
    }
}
