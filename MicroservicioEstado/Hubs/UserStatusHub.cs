using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MicroservicioEstado.Hubs
{
    public class UserStatusHub: Hub
    {
        public async Task SendStatus(string username, string status)
        {
            await Clients.All.SendAsync("ReceiveStatus", username, status);
        }
    }
}
