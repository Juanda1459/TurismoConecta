namespace TurismoConecta.api.Models
{
    public partial class SitioTuristico
    {
        public int IdSitioTuristico { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? ImagenUrl { get; set; }
        public int IdMunicipio { get; set; }
        public int? IdReseña { get; set; }

        public virtual Municipio IdMunicipioNavigation { get; set; } = null!;
        public virtual Reseña? IdReseñaNavigation { get; set; }
    }
}