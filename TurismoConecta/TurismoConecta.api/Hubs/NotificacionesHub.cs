
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace TurismoConecta.api.Hubs
{
    [Authorize]
    public class NotificacionesHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var idUsuario = Context.User?.FindFirst("nameid")?.Value
                ?? Context.User?.Identity?.Name;

            if (!string.IsNullOrEmpty(idUsuario))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"usuario-{idUsuario}");
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var idUsuario = Context.User?.FindFirst("nameid")?.Value
                ?? Context.User?.Identity?.Name;

            if (!string.IsNullOrEmpty(idUsuario))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"usuario-{idUsuario}");
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task EnviarNotificacionPrueba(string mensaje)
        {
            await Clients.Caller.SendAsync("RecibirNotificacion", $"Eco del servidor: {mensaje}");
        }
    }
}