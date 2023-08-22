
using Orleans;
using Orleans.Netcode.Net;
using System.Collections.Generic;

namespace Netcode.Orleans
{
    [Immutable, GenerateSerializer]
    public sealed record AllMessage([Immutable] InvocationMessage Message, [Immutable] IReadOnlyList<string>? ExcludedIds = null);

}
