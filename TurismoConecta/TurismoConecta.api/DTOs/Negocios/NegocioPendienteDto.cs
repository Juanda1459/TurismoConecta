namespace TurismoConecta.api.DTOs.Negocios
{
    // DTO liviano para HU-24: la bandeja de pendientes no necesita galería ni promedio,
    // solo lo esencial para que el admin municipal decida rápido
    public class NegocioPendienteDto
    {
        public int IdNegocio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int IdCategoria { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string NombrePropietario { get; set; } = string.Empty;
        public int IdMunicipio { get; set; }
        public string NombreMunicipio { get; set; } = string.Empty;
        public string NombreCategoria { get; set; } = string.Empty;


    }

}
