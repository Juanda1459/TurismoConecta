// DTOs/Itinerarios/ItinerarioResponseDto.cs
namespace TurismoConecta.api.DTOs.Itinerarios
{
    public class ItinerarioResponseDto
    {
        public int IdItinerario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public bool Compartido { get; set; }
        public Guid CodigoCompartir { get; set; }
        public string? Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
        public List<ParadaResponseDto> Paradas { get; set; } = new();
    }
}