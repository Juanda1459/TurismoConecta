using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace TurismoConecta.web.Client.Services
{
    public class JwtAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _js;

        public JwtAuthStateProvider(IJSRuntime js)
        {
            _js = js;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            string? token = null;

            try
            {
                token = await _js.InvokeAsync<string?>("localStorage.getItem", "authToken");
            }
            catch (InvalidOperationException)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                if (jwt.ValidTo < DateTime.UtcNow)
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                var claims = jwt.Claims.ToList();

                // Asegurar compatibilidad: si el claim se llama "role", duplicarlo como ClaimTypes.Role
                var roleClaim = claims.FirstOrDefault(c => c.Type == "role" || c.Type == ClaimTypes.Role);
                if (roleClaim != null && roleClaim.Type != ClaimTypes.Role)
                {
                    claims.Add(new Claim(ClaimTypes.Role, roleClaim.Value));
                }

                var identity = new ClaimsIdentity(claims, "jwt", ClaimTypes.Name, ClaimTypes.Role);
                var user = new ClaimsPrincipal(identity);

                return new AuthenticationState(user);
            }
            catch
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        public void NotificarCambioAutenticacion()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}