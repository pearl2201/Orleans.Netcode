using System;
using Netcode.Orleans.Net;
using Newtonsoft.Json;
using Orleans.Netcode.Net;
using WebSocketSharp.Server;

namespace Orleans.Websocket.Net
{
    public abstract class BaseHubConnectionContext : IHubConnectionContext
    {
        public string ConnectionId { get; set; }
        public CancellationToken ConnectionAborted { get; set; }
        public bool IsAuthenticated { get; set; }
        public string UserIdentifier { get; set; }

        public abstract Task WriteAsync(InvocationMessage message);
    }

    public class WebsocketHubConnectionContext : BaseHubConnectionContext
    {
        public Action<string> WriteCallback { get; set; }

        public override Task WriteAsync(InvocationMessage message)
        {
            WriteCallback.Invoke(JsonConvert.SerializeObject(message)); 
            return Task.CompletedTask;
        }
    }
}

