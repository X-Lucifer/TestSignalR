using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace TestSignalrServer.Hubs
{
    public interface IChatHub
    {
        Task Receive(dynamic jdata);
    }
}