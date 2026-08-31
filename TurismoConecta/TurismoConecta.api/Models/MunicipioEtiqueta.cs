using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class MunicipioEtiqueta
{
    public int IdMunicipioEtiqueta { get; set; }

    public int IdMunicipio { get; set; }

    public int IdEtiqueta { get; set; }

    public virtual Etiqueta IdEtiquetaNavigation { get; set; } = null!;

    public virtual Municipio IdMunicipioNavigation { get; set; } = null!;
}
