
namespace TurismoConecta.api.DTOs.Municipios
{
    public class MunicipioCrearDto
    {
        public int IdDepartamento { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }

        public string? Historia { get; set; }

        public string? Clima { get; set; }

        public string? FechasRelevantes { get; set; }

        public string? ImagenUrl { get; set; }

        public decimal? Latitud { get; set; }

        public decimal? Longitud { get; set; }

        public DateTime FechaCreacion { get; set; }

        public bool Activo { get; set; }

    }
}
