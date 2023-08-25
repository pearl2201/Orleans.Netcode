using Microsoft.Extensions.DependencyInjection;
using Netcode.Orleans.Net;
using Orleans.Websocket.Config;
using Orleans.Websocket.Net;

namespace Netcode.Orleans.Hosting
{
    public static class IWebsocketRBuilderExtensions
    {
        public static IWebsocketBuilder AddOrleans(this IWebsocketBuilder builder)
        {
            builder.Services.AddSingleton(typeof(HubLifetimeManager<WebsocketHub, WebsocketHubConnectionContext>), typeof(OrleansHubLifetimeManagerv1<WebsocketHub, WebsocketHubConnectionContext>));
            return builder;
        }

        public static IWebsocketServerBuilder AddOrleans(this IWebsocketServerBuilder builder)
        {
            builder.Services.AddSingleton(typeof(HubLifetimeManager<WebsocketHub, WebsocketHubConnectionContext>), typeof(OrleansHubLifetimeManagerv1<WebsocketHub, WebsocketHubConnectionContext>));
            return builder;
        }
    }

}
