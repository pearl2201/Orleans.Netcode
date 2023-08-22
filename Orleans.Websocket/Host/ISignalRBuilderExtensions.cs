using Microsoft.Extensions.DependencyInjection;
using Netcode.Orleans.Net;
using Orleans.Websocket.Config;

namespace Netcode.Orleans.Hosting
{
    public static class IWebsocketRBuilderExtensions
    {
        public static IWebsocketBuilder AddOrleans(this IWebsocketBuilder builder)
        {
            builder.Services.AddSingleton(typeof(HubLifetimeManager<>), typeof(OrleansHubLifetimeManager<>));
            return builder;
        }

        public static IWebsocketServerBuilder AddOrleans(this IWebsocketServerBuilder builder)
        {
            builder.Services.AddSingleton(typeof(HubLifetimeManager<>), typeof(OrleansHubLifetimeManager<>));
            return builder;
        }
    }

}
