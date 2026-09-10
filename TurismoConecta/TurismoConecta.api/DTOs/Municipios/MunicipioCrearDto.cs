namespace TurismoConecta.api.DTOs.Municipios
{
    public class MunicipioCrearDto
    {
        public int IdDepartamento { get; set; }
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public string? Historia { get; set; }
        public string? Clima { get; set; }
        public string? ImagenUrl { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }

        // Ahora es una lista completa de festividades:
        public List<FechaRelevanteDto> FechasRelevantes { get; set; } = new();

        public List<string> Etiquetas { get; set; } = new();
    }
}