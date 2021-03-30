using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SSocial.Hubs
{
    public class NotifHub : Hub
    {
        public async Task Notify(string user, string data)
        {
            await Clients.All.SendAsync("ReceiveNotif", data);
        }
    }
}