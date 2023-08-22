using Netcode.Orleans.Net;

namespace Netcode.Orleans
{
    internal static class HubUtility
    {
        internal static string GetHubName<THub>() where THub : IHub
        {
            return typeof(THub).Name;
        }
    }
}
