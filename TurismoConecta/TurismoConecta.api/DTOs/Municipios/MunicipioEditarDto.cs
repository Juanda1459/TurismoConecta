using System.ComponentModel.DataAnnotations;

namespace TurismoConecta.api.DTOs.Municipios
{
    public class MunicipioEditarDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Descripcion { get; set; }

        [MaxLength(200)]
        public string? Clima { get; set; }

        public string? Historia { get; set; }

        [MaxLength(500)]
        public string? FechasRelevantes { get; set; }
    }
}