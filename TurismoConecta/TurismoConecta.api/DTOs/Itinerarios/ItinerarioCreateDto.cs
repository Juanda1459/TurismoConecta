using System.ComponentModel.DataAnnotations;

namespace TurismoConecta.api.DTOs.Itinerarios
{
    public class ItinerarioCreateDto
    {
        [Required(ErrorMessage = "El nombre del itinerario es obligatorio.")]
        [MaxLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        public DateOnly? FechaInicio { get; set; }

        public DateOnly? FechaFin { get; set; }

        [MaxLength(500, ErrorMessage = "Las observaciones no pueden superar los 500 caracteres.")]
        public string? Observaciones { get; set; }

        [MinLength(1, ErrorMessage = "El itinerario debe tener al menos una parada.")]
        public List<ItinerarioParadaDto> Paradas { get; set; } = new();
    }
}
