using System.Net.Http.Json;

namespace TurismoConecta.web.Client.Services
{
    public class MunicipioApiService
    {
        private readonly HttpClient _http;
        public MunicipioApiService(HttpClient http) => _http = http;

        public async Task<ResultadoPaginado<MunicipioListadoDto>?> ListarAsync(int pagina, int tamano, CancellationToken ct = default)
        {
            try
            {
                return await _http.GetFromJsonAsync<ResultadoPaginado<MunicipioListadoDto>>(
                    $"api/municipios?pagina={pagina}&tamano={tamano}", ct);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                return null; // el componente decide qué mostrar (mensaje de error) si recibe null
            }
        }

        public async Task<List<MunicipioListadoDto>?> BuscarAsync(string texto, CancellationToken ct = default)
        {
            try
            {
                return await _http.GetFromJsonAsync<List<MunicipioListadoDto>>(
                    $"api/municipios/buscar?texto={Uri.EscapeDataString(texto)}", ct);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                return null;
            }
        }

        public async Task<MunicipioFichaDto?> ObtenerFichaAsync(int id, CancellationToken ct = default)
        {
            try
            {
                return await _http.GetFromJsonAsync<MunicipioFichaDto>($"api/municipios/{id}", ct);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                return null;
            }
        }

        public async Task<bool> EditarAsync(int id, MunicipioEditarDto dto, CancellationToken ct = default)
        {
            var respuesta = await _http.PutAsJsonAsync($"api/municipios/{id}", dto, ct);
            return respuesta.IsSuccessStatusCode;
        }

        public async Task<List<EtiquetaDto>?> ListarEtiquetasAsync(CancellationToken ct = default)
        {
            try { return await _http.GetFromJsonAsync<List<EtiquetaDto>>("api/etiquetas", ct); }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) { return null; }
        }

        public async Task<List<MunicipioListadoDto>?> BuscarAsync(string? texto, int? idEtiqueta, CancellationToken ct = default)
        {
            var query = new List<string>();
            if (!string.IsNullOrWhiteSpace(texto)) query.Add($"texto={Uri.EscapeDataString(texto)}");
            if (idEtiqueta.HasValue) query.Add($"idEtiqueta={idEtiqueta}");
            var qs = query.Any() ? "?" + string.Join("&", query) : "";

            try { return await _http.GetFromJsonAsync<List<MunicipioListadoDto>>($"api/municipios/buscar{qs}", ct); }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException) { return null; }
        }
    }

    public class ResultadoPaginado<T>
    {
        public List<T> Items { get; set; } = new();
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int Tamano { get; set; }
        public int TotalPaginas { get; set; }
    }

    public class EtiquetaDto
    {
        public int IdEtiqueta { get; set; }
        public string Nombre { get; set; } = "";
    }

    public class MunicipioListadoDto
    {
        public int IdMunicipio { get; set; }
        public string Nombre { get; set; } = "";
        public string? ImagenUrl { get; set; }
        public List<string> Etiquetas { get; set; } = new();
    }

    public class MunicipioFichaDto : MunicipioListadoDto
    {
        public string? Descripcion { get; set; }
        public string? Clima { get; set; }
        public string? Historia { get; set; }
        public string? FechasRelevantes { get; set; }
    }

    public class MunicipioEditarDto
    {
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public string? Clima { get; set; }
        public string? Historia { get; set; }
        public string? FechasRelevantes { get; set; }
    }
}