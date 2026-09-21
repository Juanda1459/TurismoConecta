namespace TurismoConecta.api.DTOs.SitiosTuristicos
{
    public class SitioTuristicoCrearDto
    {
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public string? ImagenUrl { get; set; }
        public int IdMunicipio { get; set; }
    }

    public class SitioTuristicoEditarDto
    {
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public string? ImagenUrl { get; set; }
    }
}