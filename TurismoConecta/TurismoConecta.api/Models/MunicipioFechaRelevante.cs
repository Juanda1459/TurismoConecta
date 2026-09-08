using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class MunicipioFechaRelevante
{
    public int IdMunicipioFechaRelevante { get; set; }

    public int IdMunicipio { get; set; }

    public int IdFechaRelevante { get; set; }

    public string? FotoUrl { get; set; }

    public bool EsFestividadPrincipal { get; set; }

    public string? DescripcionLocal { get; set; }

    public int Orden { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual FechaRelevante IdFechaRelevanteNavigation { get; set; } = null!;

    public virtual Municipio IdMunicipioNavigation { get; set; } = null!;
}
