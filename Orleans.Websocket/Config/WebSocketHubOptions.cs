using System;
using WebSocketSharp.Server;

namespace Orleans.Websocket.Config
{
	public class WebSocketHubOptions
	{
		private IDictionary<string, Type> hubMap { get; } = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);

		public void AddHub<T>(string name) where T: WebSocketBehavior
		{
			if (name == null)
			{
				throw new ArgumentException(nameof(T));
			}

			hubMap[name] = typeof(T);
		}

		public Type GetHub(string name)
		{
			if (name == null)

			{
				throw new ArgumentException(name);
			}

			return hubMap.ContainsKey(name) ? hubMap[name] : null;
		}

    }
}

