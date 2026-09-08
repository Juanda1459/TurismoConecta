using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class FechaRelevante
{
    public int IdFechaRelevante { get; set; }

    public string NombreFestividad { get; set; } = null!;

    public DateOnly FechaInicio { get; set; }

    public DateOnly FechaFin { get; set; }

    public string? Descripcion { get; set; }

    public string? Historia { get; set; }

    public bool EsRecurrente { get; set; }

    public int? MesCelebracion { get; set; }

    public string? TipoFestividad { get; set; }

    public string? Recomendaciones { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<MunicipioFechaRelevante> MunicipioFechaRelevantes { get; set; } = new List<MunicipioFechaRelevante>();
}
