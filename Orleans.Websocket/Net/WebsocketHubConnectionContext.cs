using System;
using Netcode.Orleans.Net;
using Orleans.Netcode.Net;
using WebSocketSharp.Server;

namespace Orleans.Websocket.Net
{
	public class WebsocketHubConnectionContext: HubConnectionContext
	{
       

        public override Task WriteAsync(InvocationMessage message)
        {
            throw new NotImplementedException();
        }
    }
}

