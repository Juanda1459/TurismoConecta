using System.Net.Http.Json;

namespace TurismoConecta.web.Client.Services
{
    public class EtiquetaApiService
    {
        private readonly HttpClient _http;
        public EtiquetaApiService(HttpClient http) => _http = http;

        public async Task<List<EtiquetaDto>> ListarAsync(CancellationToken ct = default)
        {
            try
            {
                return await _http.GetFromJsonAsync<List<EtiquetaDto>>("api/etiquetas", ct) ?? new();
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                return new();
            }
        }
    }
}