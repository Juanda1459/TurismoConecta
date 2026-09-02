using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class Negocio
{
    public int IdNegocio { get; set; }

    public int IdMunicipio { get; set; }

    public int IdCategoria { get; set; }

    public int? IdUsuario { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public string? Email { get; set; }

    public string? Horario { get; set; }

    public string? ImagenPrincipalUrl { get; set; }

    public decimal? Latitud { get; set; }

    public decimal? Longitud { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime? FechaRegistro { get; set; }

    public DateTime? FechaAprobacion { get; set; }

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    public virtual ICollection<GaleriaNegocio> GaleriaNegocios { get; set; } = new List<GaleriaNegocio>();

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual Municipio IdMunicipioNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    public virtual ICollection<NegocioEtiqueta> NegocioEtiqueta { get; set; } = new List<NegocioEtiqueta>();

    public virtual ICollection<Reseña> Reseñas { get; set; } = new List<Reseña>();
}
