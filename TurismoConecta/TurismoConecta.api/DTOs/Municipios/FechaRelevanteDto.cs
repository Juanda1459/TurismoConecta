namespace TurismoConecta.api.DTOs.Municipios
{
    public class FechaRelevanteDto
    {
        public string NombreFestividad { get; set; } = string.Empty;
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public string? Descripcion { get; set; }
        public bool EsRecurrente { get; set; }
    }
}