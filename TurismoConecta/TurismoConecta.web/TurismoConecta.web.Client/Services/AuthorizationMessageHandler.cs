using System.Net.Http.Headers;
using Microsoft.JSInterop;

namespace TurismoConecta.web.Client.Services
{
    // Este handler se "engancha" al HttpClient y se ejecuta ANTES de cada petición,
    // agregando el header Authorization: Bearer {token} automáticamente.
    // Así ningún componente tiene que acordarse de hacerlo manualmente cada vez.
    public class AuthorizationMessageHandler : DelegatingHandler
    {
        private readonly IJSRuntime _js;
        public AuthorizationMessageHandler(IJSRuntime js) => _js = js;

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string? token = null;

            try
            {
                token = await _js.InvokeAsync<string?>("localStorage.getItem", "authToken");
            }
            catch (InvalidOperationException)
            {
                // Estamos en la fase de prerenderizado (servidor, sin navegador todavía).
                // No hay token disponible en este momento — la petición sigue sin autenticar.
                // Cuando WebAssembly termine de cargar, las siguientes peticiones sí van a
                // encontrar el token normalmente.
            }

            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}