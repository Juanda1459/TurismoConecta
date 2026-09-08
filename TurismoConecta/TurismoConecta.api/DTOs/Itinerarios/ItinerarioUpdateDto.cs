using System.ComponentModel.DataAnnotations;

namespace TurismoConecta.api.DTOs.Itinerarios
{
    public class ItinerarioUpdateDto
    {
        [Required]
        [MaxLength(150)]
        public string Nombre { get; set; } = string.Empty;

        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }

        [MaxLength(500)]
        public string? Observaciones { get; set; }

        [MinLength(1, ErrorMessage = "Debe tener al menos una parada.")]
        public List<ItinerarioParadaDto> Paradas { get; set; } = new();
    }
}