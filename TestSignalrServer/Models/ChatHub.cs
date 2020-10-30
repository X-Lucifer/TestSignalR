using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;
using TestSignalrServer.Hubs;

namespace TestSignalrServer.Models
{
    public class ChatHub : Hub<IChatHub>
    {
        public async Task Send(dynamic jdata)
        {
            await Clients.Others.Receive(jdata);
        }
    }
}