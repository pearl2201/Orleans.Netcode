using System;
using Netcode.Orleans;
using Netcode.Orleans.Net;
using WebSocketSharp.Server;

namespace Orleans.Websocket.Net
{
	public class WebsocketHub: WebSocketBehavior,IHub
    {
		private readonly OrleansHubLifetimeManagerv1<WebsocketHub, WebsocketHubConnectionContext> orleansHubLifetimeManager;

        public WebsocketHub(OrleansHubLifetimeManagerv1<WebsocketHub, WebsocketHubConnectionContext> orleansHubLifetimeManager)
		{
		}

		
	}
}

