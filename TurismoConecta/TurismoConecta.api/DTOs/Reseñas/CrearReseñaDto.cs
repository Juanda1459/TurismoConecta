using System.ComponentModel.DataAnnotations;
namespace TurismoConecta.api.DTOs.Reseñas
{
    public class CrearReseñaDto
    {
        public int? IdMunicipio { get; set; }
        public int? IdNegocio { get; set; }

        [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5")]
        public int Calificacion { get; set; }

        [MaxLength(1000)]
        public string? Comentario { get; set; }
    }
}

