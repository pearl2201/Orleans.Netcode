using Netcode.Orleans.Core;
using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netcode.Orleans.Clients
{
    /// <summary>
    /// A single connection
    /// </summary>
    public interface IClientGrain : IHubMessageInvoker, IGrainWithStringKey
    {
        Task OnConnect(Guid serverId);
        Task OnDisconnect(string? reason = null);
    }
}
