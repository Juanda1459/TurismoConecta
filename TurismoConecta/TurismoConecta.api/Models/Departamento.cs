using System;
using System.Collections.Generic;

namespace TurismoConecta.api.Models;

public partial class Departamento
{
    public int IdDepartamento { get; set; }

    public string Nombre { get; set; } = null!;

    public string Pais { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual ICollection<Municipio> Municipios { get; set; } = new List<Municipio>();
}
