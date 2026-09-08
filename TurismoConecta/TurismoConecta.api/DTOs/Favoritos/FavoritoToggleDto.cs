namespace TurismoConecta.api.DTOs.Favoritos
{
    public class FavoritoToggleDto
    {
        public int? IdMunicipio { get; set; }
        public int? IdNegocio { get; set; }
    }

    // HU-27: "la lista de favoritos es accesible desde el perfil del usuario" — un solo endpoint
    // que trae municipios Y negocios favoritos juntos, para pintar la pantalla de perfil de una vez.
    public class FavoritoDto
    {
        public int? IdMunicipio { get; set; }
        public string? NombreMunicipio { get; set; }
        public int? IdNegocio { get; set; }
        public string? NombreNegocio { get; set; }
        public DateTime FechaGuardado { get; set; }
    }
}