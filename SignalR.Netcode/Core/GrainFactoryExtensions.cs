using Netcode.Orleans.Clients;
using Netcode.Orleans.ConnectionGroups;
using Netcode.Orleans.Net;
using Netcode.Orleans.ServerDirectory;
using Orleans;

namespace Netcode.Orleans.Core
{
    public static class GrainFactoryExtensions
    {
        public static NetContext<THub> GetHub<THub>(this IGrainFactory grainFactory) where THub : IHub
        {
            return new NetContext<THub>(grainFactory);
        }

        internal static IClientGrain GetClientGrain(this IGrainFactory factory, string hubName, string connectionId)
        {
            var key = new ClientKey { HubType = hubName, ConnectionId = connectionId }.ToGrainPrimaryKey();
            return factory.GetGrain<IClientGrain>(key);
        }

        internal static IConnectionGroupGrain GetGroupGrain(this IGrainFactory factory, string hubName, string groupName)
        {
            var key = new ConnectionGroupKey { GroupId = groupName, HubType = hubName, GroupType = ConnectionGroupType.NamedGroup }.ToPrimaryGrainKey();
            return factory.GetGrain<IConnectionGroupGrain>(key);
        }

        internal static IConnectionGroupGrain GetUserGrain(this IGrainFactory factory, string hubName, string userId)
        {
            var key = new ConnectionGroupKey { GroupId = userId, HubType = hubName, GroupType = ConnectionGroupType.AuthenticatedUser }.ToPrimaryGrainKey();
            return factory.GetGrain<IConnectionGroupGrain>(key);
        }

        internal static IServerDirectoryGrain GetServerDirectoryGrain(this IGrainFactory factory)
            => factory.GetGrain<IServerDirectoryGrain>(0);
    }

}
