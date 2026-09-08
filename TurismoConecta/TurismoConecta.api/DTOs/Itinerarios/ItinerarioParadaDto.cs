
namespace TurismoConecta.api.DTOs.Itinerarios
{
    public class ItinerarioParadaDto
    {
        public int IdMunicipio { get; set; }
        public int DiaNumero { get; set; }
        public int Orden { get; set; }
        public DateOnly? FechaVisita { get; set; }
    }
}