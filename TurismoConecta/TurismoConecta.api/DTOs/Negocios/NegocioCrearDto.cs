using System.ComponentModel.DataAnnotations;

namespace TurismoConecta.api.DTOs.Negocios
{
    public class NegocioCrearDto
    {
        [Required, MaxLength(150)] public string Nombre { get; set; } = string.Empty;
        [MaxLength(1000)] public string? Descripcion { get; set; }
        [Required] public int IdCategoria { get; set; }
        [Required] public int IdMunicipio { get; set; }
        [MaxLength(30)] public string? Telefono { get; set; }
        [MaxLength(200)] public string? Horario { get; set; }
        [MaxLength(250)] public string? Direccion { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }

        // HU-22: "galería de fotos" en el registro — lista de URLs ya subidas (el subir el archivo
        // Aquí solo se guardan las URLs resultantes)
        public List<string> ImagenesGaleria { get; set; } = new();
    }

    public class NegocioEditarDto : NegocioCrearDto { }
}