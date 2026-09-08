// DTOs/Itinerarios/ItinerarioListadoDto.cs
namespace TurismoConecta.api.DTOs.Itinerarios
{
    public class ItinerarioListadoDto
    {
        public int IdItinerario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public int CantidadParadas { get; set; }
    }
}