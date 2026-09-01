namespace TurismoConecta.api.DTOs;

using System.ComponentModel.DataAnnotations;
public class EtiquetaDto
{
    public int IdEtiqueta { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
}

public class EtiquetaCrearDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? Descripcion { get; set; }
}

public class EtiquetaActualizarDto
{
    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(300)]
    public string? Descripcion { get; set; }
}