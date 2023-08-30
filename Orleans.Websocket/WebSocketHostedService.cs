using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orleans.Websocket.Config;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Orleans.Websocket
{
    public class WebSocketHostedService : IHostedService
    {
        private readonly ILogger<WebSocketHostedService> _logger;   
        private readonly WebSocketServer _socketServer;
        private readonly IServiceProvider _serviceProvider;

        public WebSocketServer WSS
        {
            get
            {
                return _socketServer;
            }
        }

        public IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider;
            }
        }
        public WebSocketHostedService(ILogger<WebSocketHostedService> logger, IOptions<WebsocketServerHubOptions> serverHubOptions, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _socketServer = new WebSocketServer(serverHubOptions.Value.Port);
            _serviceProvider = serviceProvider;
            foreach (var act in serverHubOptions.Value.Configure)
            {
                act(this);
            }
        }





        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("[******] Start websocket server at port: " + _socketServer.Port);
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

