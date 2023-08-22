
using Orleans;
using Orleans.Netcode.Net;

namespace Netcode.Orleans
{
    [Immutable, GenerateSerializer]
    public sealed record ClientMessage(string HubName, string ConnectionId, [Immutable] InvocationMessage Message);

}
