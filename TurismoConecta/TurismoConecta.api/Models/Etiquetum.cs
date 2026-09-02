using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class Etiquetum
{
    public int IdEtiqueta { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<MunicipioEtiquetum> MunicipioEtiqueta { get; set; } = new List<MunicipioEtiquetum>();

    public virtual ICollection<NegocioEtiquetum> NegocioEtiqueta { get; set; } = new List<NegocioEtiquetum>();
}
