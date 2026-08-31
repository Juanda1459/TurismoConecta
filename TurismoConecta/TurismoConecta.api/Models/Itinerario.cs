using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class Itinerario
{
    public int IdItinerario { get; set; }

    public int IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public bool Compartido { get; set; }

    public Guid CodigoCompartir { get; set; }

    public DateTime FechaCreacion { get; set; }

    public string? Observaciones { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<ItinerarioDetalle> ItinerarioDetalles { get; set; } = new List<ItinerarioDetalle>();
}
