using Orleans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netcode.Orleans.ServerDirectory
{
    [GenerateSerializer]
    public sealed class ServerDirectoryState
    {
        [Id(0)]
        public Dictionary<Guid, DateTime> ServerHeartBeats { get; set; } = new();
    }

}
