using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class Municipio
{
    public int IdMunicipio { get; set; }

    public int IdDepartamento { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public string? Historia { get; set; }

    public string? Clima { get; set; }

    public string? FechasRelevantes { get; set; }

    public string? ImagenUrl { get; set; }

    public decimal? Latitud { get; set; }

    public decimal? Longitud { get; set; }

    public DateTime FechaCreacion { get; set; }

    public bool Activo { get; set; }

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    public virtual Departamento IdDepartamentoNavigation { get; set; } = null!;

    public virtual ICollection<ItinerarioDetalle> ItinerarioDetalles { get; set; } = new List<ItinerarioDetalle>();

    public virtual ICollection<MunicipioEtiquetum> MunicipioEtiqueta { get; set; } = new List<MunicipioEtiquetum>();

    public virtual ICollection<Negocio> Negocios { get; set; } = new List<Negocio>();

    public virtual ICollection<Reseña> Reseñas { get; set; } = new List<Reseña>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
