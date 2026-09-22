namespace TurismoConecta.api.DTOs.SitiosTuristicos
{
    public class SitioTuristicoDto
    {
        public int IdSitioTuristico { get; set; }
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public string? ImagenUrl { get; set; }
        public int IdMunicipio { get; set; }
        public string NombreMunicipio { get; set; } = "";
        public double? Calificacion { get; set; }
    }
}