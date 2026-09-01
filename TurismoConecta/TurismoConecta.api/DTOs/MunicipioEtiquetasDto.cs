using System.ComponentModel.DataAnnotations;

namespace TurismoConecta.api.DTOs;

public class MunicipioEtiquetasDto
{
    public int IdMunicipio { get; set; }
    public string NombreMunicipio { get; set; } = string.Empty;
    public List<EtiquetaDto> Etiquetas { get; set; } = new();
}

// Admin municipal ENVÍA: la lista completa de Ids de etiquetas que quiere que tenga su municipio
public class AsignarEtiquetasDto
{
    [MinLength(1, ErrorMessage = "Debe seleccionar al menos 1 etiqueta")]
    public List<int> IdsEtiquetas { get; set; } = new();
}