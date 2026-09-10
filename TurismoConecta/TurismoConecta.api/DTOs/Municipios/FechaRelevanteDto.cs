namespace TurismoConecta.api.DTOs.Municipios
{
    public class FechaRelevanteDto
    {
        public int? IdFechaRelevante { get; set; }
        public string NombreFestividad { get; set; } = string.Empty;
        public DateOnly FechaInicio { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public DateOnly FechaFin { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public string? TipoFestividad { get; set; } = "Cultural";
        public int? MesCelebracion { get; set; }
        public string? Descripcion { get; set; }
        public bool EsRecurrente { get; set; } = true;
    }
}