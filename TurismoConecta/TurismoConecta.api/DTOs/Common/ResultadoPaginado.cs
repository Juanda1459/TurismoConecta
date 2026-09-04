namespace TurismoConecta.api.DTOs.Common
{
    public class ResultadoPaginado<T>
    {
        public List<T> Items { get; set; } = new();
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int Tamano { get; set; }
        public int TotalPaginas => Tamano == 0 ? 0 : (int)Math.Ceiling((double)Total / Tamano);
    }
}