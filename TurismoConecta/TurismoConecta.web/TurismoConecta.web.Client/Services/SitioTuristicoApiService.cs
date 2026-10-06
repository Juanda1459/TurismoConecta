using System.Net.Http.Json;
using TurismoConecta.web.Client.Models;

namespace TurismoConecta.web.Client.Services
{
    /// <summary>
    /// Habla con api/sitios-turisticos y api/categorias/sitios.
    /// Lo usa Inicio.razor para pintar las cards de "Maravillas de Boyacá".
    /// </summary>
    public class SitioTuristicoApiService
    {
        private readonly HttpClient _http;
        public SitioTuristicoApiService(HttpClient http) => _http = http;

        // ── LECTURA (pública) ──────────────────────────────

        public async Task<List<SitioTuristicoDto>> ListarAsync(
            bool soloDestacados = false,
            int? idCategoria = null,
            CancellationToken ct = default)
        {
            var url = $"api/sitios-turisticos?soloDestacados={soloDestacados.ToString().ToLower()}";
            if (idCategoria.HasValue)
                url += $"&idCategoria={idCategoria.Value}";

            try
            {
                return await _http.GetFromJsonAsync<List<SitioTuristicoDto>>(url, ct) ?? new();
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                Console.WriteLine($"[SitioTuristicoApiService] No se pudieron cargar los sitios: {ex.Message}");
                return new();
            }
        }

        public async Task<List<CategoriaDto>> ListarCategoriasAsync(CancellationToken ct = default)
        {
            try
            {
                return await _http.GetFromJsonAsync<List<CategoriaDto>>("api/categorias/sitios", ct) ?? new();
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                Console.WriteLine($"[SitioTuristicoApiService] No se pudieron cargar las categorías: {ex.Message}");
                return new();
            }
        }

        // ── ESCRITURA (solo administradores) ───────────────

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

    /// <summary>
    /// Copia exacta del SitioTuristicoDto de la API.
    /// Si un nombre no coincide, ese dato llega vacío SIN dar error.
    /// </summary>
    public class SitioTuristicoDto
    {
        public int IdSitioTuristico { get; set; }
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public string? ImagenUrl { get; set; }

        public int IdMunicipio { get; set; }
        public string NombreMunicipio { get; set; } = "";
        public string? Clima { get; set; }

        public int? Altitud { get; set; }
        public bool Destacado { get; set; }

        public int? IdCategoria { get; set; }
        public string? NombreCategoria { get; set; }
        public string? IconoCategoria { get; set; }

        public double? Calificacion { get; set; }
    }
}
