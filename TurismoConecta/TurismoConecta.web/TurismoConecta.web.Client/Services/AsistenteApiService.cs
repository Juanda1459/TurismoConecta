using System.Net.Http.Json;

namespace TurismoConecta.web.Client.Services
{
    public class AsistenteApiService
    {
        private readonly HttpClient _http;
        public AsistenteApiService(HttpClient http) => _http = http;

        public async Task<string> EnviarMensajeAsync(string mensaje, List<TurnoChat> historial, CancellationToken ct = default)
        {
            try
            {
                var respuesta = await _http.PostAsJsonAsync("api/asistente/mensaje", new { Mensaje = mensaje, Historial = historial }, ct);
                if (!respuesta.IsSuccessStatusCode) return "No pude responder en este momento.";
                var resultado = await respuesta.Content.ReadFromJsonAsync<ChatRespuestaDto>(cancellationToken: ct);
                return resultado?.Respuesta ?? "No pude responder en este momento.";
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
            {
                return "El Cóndor Guía no está disponible en este momento.";
            }
        }
    }

    public class TurnoChat
    {
        public string Rol { get; set; } = "user";
        public string Contenido { get; set; } = "";
    }

    public class ChatRespuestaDto
    {
        public string Respuesta { get; set; } = "";
    }
}