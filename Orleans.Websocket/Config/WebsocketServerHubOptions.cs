using System;
using WebSocketSharp.Server;

namespace Orleans.Websocket.Config
{
	public class WebsocketServerHubOptions
	{
		public int Port { get; set; }

       

		public Action<WebSocketServer> Configure { get; set; }

        public IDictionary<string, Type> hubMap { get; } = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

    }
}

