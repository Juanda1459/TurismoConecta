using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class GaleriaNegocio
{
    public int IdGaleria { get; set; }

    public int IdNegocio { get; set; }

    public string ImagenUrl { get; set; } = null!;

    public virtual Negocio IdNegocioNavigation { get; set; } = null!;
}
