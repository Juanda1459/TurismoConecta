namespace TurismoConecta.api.DTOs.Municipios
{
    public class MunicipioFichaDto : MunicipioListadoDto
    {
        public string? Descripcion { get; set; }
        public string? Clima { get; set; }
        public string? Historia { get; set; }
        public string? FechasRelevantes { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
    }
}