using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class Etiqueta
{
    public int IdEtiqueta { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }
    public bool Activo { get; set; }

    public virtual ICollection<MunicipioEtiqueta> MunicipioEtiqueta { get; set; } = new List<MunicipioEtiqueta>();

    public virtual ICollection<NegocioEtiqueta> NegocioEtiqueta { get; set; } = new List<NegocioEtiqueta>();
}
