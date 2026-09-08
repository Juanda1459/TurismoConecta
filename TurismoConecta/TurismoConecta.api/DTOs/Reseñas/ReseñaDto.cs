namespace TurismoConecta.api.DTOs.Reseñas
{

    public class ReseñaDto
    {
        public int IdReseña { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int Calificacion { get; set; }
        public string? Comentario { get; set; }
        public string? Respuesta { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}