using Microsoft.Extensions.DependencyInjection;
using Netcode.Orleans.Config;
using Orleans.Websocket.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;

namespace Netcode.Orleans.Hosting
{
    public static class IClientBuilderExtensions
    {
        public static IClientBuilder UseWebsocket(this IClientBuilder builder, Action<NetClientConfig>? configure = null)
        {
            var cfg = new NetClientConfig();
            configure?.Invoke(cfg);
            return builder.UseWebsocket(cfg);
        }

        public static IClientBuilder UseWebsocket(this IClientBuilder builder, NetClientConfig? config = null)
        {
            config ??= new NetClientConfig();
            return builder.AddMemoryStreams(SignalROrleansConstants.SIGNALR_ORLEANS_STREAM_PROVIDER);
        }

        
    }

}
