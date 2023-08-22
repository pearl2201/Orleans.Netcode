using Orleans;
using System;
using System.Threading.Tasks;

namespace Netcode.Orleans.ServerDirectory
{
    public interface IServerDirectoryGrain : IGrainWithIntegerKey
    {
        Task Heartbeat(Guid serverId);
        Task Unregister(Guid serverId);
    }

}
