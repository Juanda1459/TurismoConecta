using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class Reseña
{
    public int IdReseña { get; set; }

    public int IdUsuario { get; set; }

    public int? IdMunicipio { get; set; }

    public int? IdNegocio { get; set; }

    public byte Calificacion { get; set; }

    public string? Comentario { get; set; }

    public string? Respuesta { get; set; }

    public DateTime? FechaRespuesta { get; set; }

    public bool Moderada { get; set; }

    public DateTime FechaCreacion { get; set; }

    public virtual Municipio? IdMunicipioNavigation { get; set; }

    public virtual Negocio? IdNegocioNavigation { get; set; }

    public virtual Usuario IdUsuarioNavigation { get; set; } = null!;
}
