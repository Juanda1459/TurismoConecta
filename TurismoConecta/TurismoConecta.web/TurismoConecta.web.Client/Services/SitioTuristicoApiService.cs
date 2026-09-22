using System.Net.Http.Json;

namespace TurismoConecta.web.Client.Services
{
    public class SitioTuristicoApiService
    {
        private readonly HttpClient _http;
        public SitioTuristicoApiService(HttpClient http) => _http = http;

        public async Task<List<SitioTuristicoDto>> ListarAsync(CancellationToken ct = default)
        {
            try { return await _http.GetFromJsonAsync<List<SitioTuristicoDto>>("api/sitios-turisticos", ct) ?? new(); }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) { return new(); }
        }

        public async Task<(bool exito, int id, string? error)> CrearAsync(object dto, CancellationToken ct = default)
        {
            var respuesta = await _http.PostAsJsonAsync("api/sitios-turisticos", dto, ct);
            if (respuesta.IsSuccessStatusCode)
            {
                var resultado = await respuesta.Content.ReadFromJsonAsync<Dictionary<string, int>>(cancellationToken: ct);
                return (true, resultado?["idSitioTuristico"] ?? 0, null);
            }
            return (false, 0, await respuesta.Content.ReadAsStringAsync(ct));
        }

        public async Task<bool> EditarAsync(int id, object dto, CancellationToken ct = default)
        {
            var respuesta = await _http.PutAsJsonAsync($"api/sitios-turisticos/{id}", dto, ct);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarAsync(int id, CancellationToken ct = default)
        {
            var respuesta = await _http.DeleteAsync($"api/sitios-turisticos/{id}", ct);
            return respuesta.IsSuccessStatusCode;
        }
    }

    public class SitioTuristicoDto
    {
        public int IdSitioTuristico { get; set; }
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public string? ImagenUrl { get; set; }
        public int IdMunicipio { get; set; }
        public string NombreMunicipio { get; set; } = "";
        public double? Calificacion { get; set; }
    }
}