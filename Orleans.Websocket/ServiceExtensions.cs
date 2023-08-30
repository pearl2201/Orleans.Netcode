using System;
using Microsoft.Extensions.DependencyInjection;
using Orleans.Websocket.Config;
using WebSocketSharp.Server;

namespace Orleans.Websocket
{
    public static class ServiceExtensions
    {
        public static IWebsocketServerBuilder AddWebsocket(this IServiceCollection services, Action<WebsocketServerHubOptions>? configure = null)
        {
            services.AddHostedService<WebSocketHostedService>();
            WebsocketServerHubOptions opts = new WebsocketServerHubOptions();

            services.Configure<WebsocketServerHubOptions>(configure);
            return new IWebsocketServerBuilder()
            {
                Services = services
            };
        }
    }
}

