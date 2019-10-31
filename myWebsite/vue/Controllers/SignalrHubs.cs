using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace vue.Controllers
{
    public class SignalrHubs : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}
