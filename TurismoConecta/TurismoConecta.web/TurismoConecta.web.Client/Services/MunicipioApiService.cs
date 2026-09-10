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

        public async Task<(bool exito, int idCreado, string? error)> CrearAsync(MunicipioCrearDto dto, CancellationToken ct = default)
        {
            var respuesta = await _http.PostAsJsonAsync("api/municipios", dto, ct);
            if (respuesta.IsSuccessStatusCode)
            {
                var resultado = await respuesta.Content.ReadFromJsonAsync<CrearResultado>(cancellationToken: ct);
                return (true, resultado?.IdMunicipio ?? 0, null);
            }
            var error = await respuesta.Content.ReadAsStringAsync(ct);
            return (false, 0, error);
        }

        private class CrearResultado { public int IdMunicipio { get; set; } }

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

        public async Task<(bool exito, string? error)> EditarAsync(int id, MunicipioEditarDto dto, CancellationToken ct = default)
        {
            var respuesta = await _http.PutAsJsonAsync($"api/municipios/{id}", dto, ct);
            if (respuesta.IsSuccessStatusCode)
            {
                return (true, null);
            }
            // Leemos el mensaje exacto que envió el servidor
            var error = await respuesta.Content.ReadAsStringAsync(ct);
            return (false, string.IsNullOrWhiteSpace(error) ? "Error al actualizar el municipio." : error);
        }

        public async Task<(bool exito, string? url, string? error)> SubirImagenAsync(Microsoft.AspNetCore.Components.Forms.IBrowserFile archivo)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                var stream = archivo.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024); // Límite de 10 MB
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(archivo.ContentType);
                content.Add(fileContent, "archivo", archivo.Name);

                var respuesta = await _http.PostAsync("api/municipios/subir-imagen", content);
                if (respuesta.IsSuccessStatusCode)
                {
                    var resultado = await respuesta.Content.ReadFromJsonAsync<RespuestaSubidaImagen>();
                    return (true, resultado?.Url, null);
                }

                var error = await respuesta.Content.ReadAsStringAsync();
                return (false, null, error);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        private class RespuestaSubidaImagen { public string Url { get; set; } = ""; }

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

        public async Task<List<NegocioResumenDto>?> ObtenerNegociosPorMunicipioAsync(int idMunicipio, CancellationToken ct = default)
        {
            try
            {
                return await _http.GetFromJsonAsync<List<NegocioResumenDto>>($"api/negocios/por-municipio/{idMunicipio}", ct);
            }
            catch (Exception)
            {
                return new List<NegocioResumenDto>();
            }
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
        public List<FechaRelevanteDto> FechasRelevantes { get; set; } = new();
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }

    }

    public class FechaRelevanteDto
    {
        public int? IdFechaRelevante { get; set; }
        public string NombreFestividad { get; set; } = "";
        public DateOnly FechaInicio { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public DateOnly FechaFin { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public string? TipoFestividad { get; set; } = "Cultural";
        public int? MesCelebracion { get; set; }
        public string? Descripcion { get; set; }
        public bool EsRecurrente { get; set; } = true;
    }

    public class MunicipioCrearDto
    {
        public int IdDepartamento { get; set; }
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public string? Historia { get; set; }
        public string? Clima { get; set; }
        public string? ImagenUrl { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
        public List<FechaRelevanteDto> FechasRelevantes { get; set; } = new();
        public List<string> Etiquetas { get; set; } = new();

    }

    public class MunicipioEditarDto
    {
        public string Nombre { get; set; } = "";
        public string? Descripcion { get; set; }
        public string? Clima { get; set; }
        public string? Historia { get; set; }
        public string? ImagenUrl { get; set; }
        public List<FechaRelevanteDto> FechasRelevantes { get; set; } = new();
        public List<string> Etiquetas { get; set; } = new();



    }

    public class NegocioResumenDto
    {
        public int IdNegocio { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string? Telefono { get; set; }
        public string? Horario { get; set; }
        public string? Direccion { get; set; }
        public double? PromedioCalificacion { get; set; }
        public List<string> Galeria { get; set; } = new();
    }


}