using Microsoft.Extensions.DependencyInjection;
using Netcode.Orleans.Net;
using Orleans.Netcode;
using Orleans.Websocket.Net;
using Microsoft.Extensions.Logging;

namespace Orleans.Websocket.Config
{
    public class WebsocketServerHubOptions
    {
        public int Port { get; set; }



        public List<Action<WebSocketHostedService>> Configure { get; set; } = new List<Action<WebSocketHostedService>>();

        public IDictionary<string, Type> hubMap { get; } = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);


        public void AddHub<THub>(string path, Action<THub> configure) where THub : WebsocketHub<THub>, IHub, new()
        {
            Configure.Add((wss) =>
            {
                wss.WSS.AddWebSocketService<THub>(path, (ws) =>
                {
                    THub temp = null;
                    var temp1 = (WebsocketHub<THub>)temp;
                    var lifetimeManager = wss.ServiceProvider.GetRequiredService<OrleansHubLifetimeManager<THub, WebsocketHubConnectionContext>>();
                    var logger = wss.ServiceProvider.GetRequiredService<ILogger<THub>>();
                    var clusterClient = wss.ServiceProvider.GetRequiredService<IClusterClient>();
                    ws.Inject(lifetimeManager, logger, clusterClient);
                });
            });

        }

    }
}

