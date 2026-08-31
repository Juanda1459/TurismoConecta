using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class NegocioEtiqueta
{
    public int IdNegocioEtiqueta { get; set; }

    public int IdEtiqueta { get; set; }

    public int IdNegocio { get; set; }

    public virtual Etiqueta IdEtiquetaNavigation { get; set; } = null!;

    public virtual Negocio IdNegocioNavigation { get; set; } = null!;
}
