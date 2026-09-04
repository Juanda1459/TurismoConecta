using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Telefono { get; set; }

    public int? MunicipioAsignadoId { get; set; }

    public DateTime FechaRegistro { get; set; }

    public bool EmailConfirmado { get; set; }

    public bool Activo { get; set; }

    public string? PasswordResetToken { get; set; }

    public DateTime? PasswordResetExpira { get; set; }

    public virtual ICollection<Favorito> Favoritos { get; set; } = new List<Favorito>();

    public virtual Rol IdRolNavigation { get; set; } = null!;

    public virtual ICollection<Itinerario> Itinerarios { get; set; } = new List<Itinerario>();

    public virtual Municipio? MunicipioAsignado { get; set; }

    public virtual ICollection<Negocio> Negocios { get; set; } = new List<Negocio>();

    public virtual ICollection<Notificacion> Notificacions { get; set; } = new List<Notificacion>();

    public virtual ICollection<Reseña> Reseñas { get; set; } = new List<Reseña>();
    public string? FotoUrl { get; set; }

}
