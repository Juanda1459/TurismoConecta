namespace TurismoConecta.api.DTOs.Municipios
{
    public class MunicipioListadoDto
    {
        public int IdMunicipio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? ImagenUrl { get; set; }
        public List<string> Etiquetas { get; set; } = new();
    }
}