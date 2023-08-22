using Orleans;
using System.Collections.Generic;

namespace Netcode.Orleans.ConnectionGroups
{
    [GenerateSerializer]
    internal sealed class ConnectionGroupGrainState
    {
        [Id(0)]
        public HashSet<string> ConnectionIds { get; set; } = new();
    }
}
