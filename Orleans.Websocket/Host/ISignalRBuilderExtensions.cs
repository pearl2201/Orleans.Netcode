using Microsoft.Extensions.DependencyInjection;
using Netcode.Orleans.Net;
using Orleans.Netcode;
using Orleans.Websocket.Config;
using Orleans.Websocket.Net;

namespace Netcode.Orleans.Hosting
{
    public static class IWebsocketRBuilderExtensions
    {
        //public static IWebsocketBuilder AddOrleans(this IWebsocketBuilder builder)
        //{
        //    builder.Services.AddSingleton(typeof(IHubLifetimeManager<WebsocketHub, WebsocketHubConnectionContext>), typeof(OrleansHubLifetimeManager<WebsocketHub, WebsocketHubConnectionContext>));
        //    return builder;
        //}

        public static IWebsocketServerBuilder AddOrleans<THub>(this IWebsocketServerBuilder builder) where THub : IHub 
        {
            builder.Services.AddSingleton(typeof(OrleansHubLifetimeManager<THub, WebsocketHubConnectionContext>));
            builder.Services.AddSingleton<IHubLifetimeManager<THub, WebsocketHubConnectionContext>>(sp =>
            {
                var service = sp.GetRequiredService<OrleansHubLifetimeManager<THub, WebsocketHubConnectionContext>>();

                return service;
            });
            return builder;
        }
    }

}
