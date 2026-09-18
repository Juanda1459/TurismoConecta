using System.Net.Http.Json;

namespace TurismoConecta.web.Client.Services
{
    public class ReseñaApiService
    {
        private readonly HttpClient _http;
        public ReseñaApiService(HttpClient http) => _http = http;

        public async Task<List<ReseñaDto>> ListarRecientesAsync(CancellationToken ct = default)
        {
            try
            {
                return await _http.GetFromJsonAsync<List<ReseñaDto>>("api/reseñas", ct) ?? new();
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                return new();
            }
        }
    }

    public class ReseñaDto
    {
        public int IdReseña { get; set; }
        public string NombreUsuario { get; set; } = "";
        public int Calificacion { get; set; }
        public string? Comentario { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}