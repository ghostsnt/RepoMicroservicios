using Microsoft.AspNetCore.SignalR;
using MicroservicioDeteccion.Service;

namespace MicroservicioDeteccion.Hubs
{
    public class PresenceHub : Hub
    {
        private readonly UserStateClient _userStateClient;

        public PresenceHub(UserStateClient client)
        {
            _userStateClient = client;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var idUsuarioStr = httpContext?.Request.Query["idUsuario"];
            var invisibleStr = httpContext?.Request.Query["invisible"].ToString();

            if (int.TryParse(idUsuarioStr, out int idUsuario))
            {
                bool isInvisible = invisibleStr == "true";
                await _userStateClient.SendState(idUsuario, "online", isInvisible);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var httpContext = Context.GetHttpContext();
            var idUsuarioStr = httpContext?.Request.Query["idUsuario"];

            if (int.TryParse(idUsuarioStr, out int idUsuario))
            {
                // Al desconectarse siempre se envía como offline
                await _userStateClient.SendState(idUsuario, "offline", false);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
