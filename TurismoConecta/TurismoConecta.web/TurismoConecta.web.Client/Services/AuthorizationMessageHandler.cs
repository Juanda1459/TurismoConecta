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
            // Ajusta "authToken" al nombre real de la clave donde tu compañero guarda el JWT
            // (revisa el AuthService/JwtService que ya tienen — probablemente usan localStorage).
            var token = await _js.InvokeAsync<string?>("localStorage.getItem", "authToken");

            if (!string.IsNullOrEmpty(token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}