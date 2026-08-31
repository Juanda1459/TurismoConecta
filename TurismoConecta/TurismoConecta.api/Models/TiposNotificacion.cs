using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class TiposNotificacion
{
    public int IdTipoNotificacion { get; set; }

    public string Codigo { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Notificacion> Notificacions { get; set; } = new List<Notificacion>();
}
