using Orleans;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netcode.Orleans.Clients
{

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    [GenerateSerializer]
    internal sealed class ClientGrainState
    {
        private string DebuggerDisplay => $"ServerId: '{ServerId}'";

        [Id(0)]
        public Guid ServerId { get; set; }
    }
}
