using Microsoft.Extensions.Logging;
using Netcode.Orleans.Net;
using Orleans.Netcode;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Orleans.Websocket.Net
{
    public class WebsocketHub<T> : WebSocketBehavior, IHub where T : WebsocketHub<T>
    {
        private OrleansHubLifetimeManager<T, WebsocketHubConnectionContext> _orleansHubLifetimeManager;
        private ILogger _logger;
        private IClusterClient _clusterClient;
        
        protected WebsocketHubConnectionContext _context;
        protected CancellationTokenSource _cts;
        public WebsocketHub()
        {
          
        }
        public OrleansHubLifetimeManager<T, WebsocketHubConnectionContext> HubLifetimeManager
        {
            get
            {
                return _orleansHubLifetimeManager;
            }
        }

        public ILogger Logger
        {
            get
            {
                return _logger;
            }
        }

        public IClusterClient ClusterClient
        {
            get
            {
                return _clusterClient;
            }
        }

        public void Inject(OrleansHubLifetimeManager<T, WebsocketHubConnectionContext> orleansHubLifetimeManager, ILogger logger, IClusterClient clusterClient)
        {
            _orleansHubLifetimeManager = orleansHubLifetimeManager;
            _logger = logger;
            _clusterClient = clusterClient;
        }
        protected override void OnOpen()
        {
            _cts = new CancellationTokenSource();
            _context = new WebsocketHubConnectionContext()
            {
                ConnectionAborted = _cts.Token,
                ConnectionId = this.ID,
                IsAuthenticated = false,
                UserIdentifier = this.ID,
                WriteCallback = (msg) =>
                {
                    this.Send(msg);
                }
            };
            _orleansHubLifetimeManager.OnConnectedAsync(_context).Wait();
            base.OnOpen();
        }

        protected override void OnClose(CloseEventArgs e)
        {
            _orleansHubLifetimeManager.OnDisconnectedAsync(_context).Wait();
            base.OnClose(e);

        }


    }
}

