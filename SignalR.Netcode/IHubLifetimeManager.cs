using System;
using Netcode.Orleans.Net;
using System.Threading.Tasks;

namespace Orleans.Netcode
{
    public interface IHubLifetimeManager<THub, THubConnectionContext> where THub:IHub where THubConnectionContext: HubConnectionContext
	{
        Task OnConnectedAsync(THubConnectionContext connection);

        Task OnDisconnectedAsync(THubConnectionContext connection);
    }
}

