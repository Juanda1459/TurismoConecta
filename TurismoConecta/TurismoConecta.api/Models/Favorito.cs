using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class Favorito
{
    public int IdFavorito { get; set; }

    public int IdUsuario { get; set; }

    public int? IdMunicipio { get; set; }

    public int? IdNegocio { get; set; }

    public DateTime FechaGuardado { get; set; }

    public virtual Municipio? IdMunicipioNavigation { get; set; }

    public virtual Negocio? IdNegocioNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
