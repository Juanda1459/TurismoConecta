using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class ItinerarioDetalle
{
    public int IdItinerarioDetalle { get; set; }

    public int IdItinerario { get; set; }

    public int IdMunicipio { get; set; }

    public int DiaNumero { get; set; }

    public int Orden { get; set; }

    public DateOnly? FechaVisita { get; set; }

    public decimal? DistanciaKm { get; set; }

    public int? TiempoEstimadoMin { get; set; }

    public virtual Itinerario IdItinerarioNavigation { get; set; } = null!;

    public virtual Municipio IdMunicipioNavigation { get; set; } = null!;
}
