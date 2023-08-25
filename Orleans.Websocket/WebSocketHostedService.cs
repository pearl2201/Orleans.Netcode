using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Orleans.Websocket.Config;
using WebSocketSharp.Server;

namespace Orleans.Websocket
{
	public class WebSocketHostedService: IHostedService
	{
        private readonly WebSocketServer _socketServer;
		public WebSocketHostedService(IOptions<WebsocketServerHubOptions> serverHubOptions)
		{
            _socketServer = new WebSocketServer(serverHubOptions.Value.Port);
            serverHubOptions.Value.Configure.Invoke(_socketServer);
            foreach(var kv in serverHubOptions.Value.hubMap)
            {
                Type tBehaviour = kv.Value;
            var typeActionBehaviour = typeof(Action<>).MakeGenericType(tBehaviour);
                var action = 
                typeof(WebSocketServer)
        .GetMethod("AddWebSocketService", new Type[] { typeof(string), typeof(string) })
        .MakeGenericMethod(tBehaviour)
        .Invoke(this, new object[] { kv.Key,  });
            }
            
        }





        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _socketServer.Start();
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Yield();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _socketServer.Stop();
            return Task.CompletedTask;
        }
    }
}

