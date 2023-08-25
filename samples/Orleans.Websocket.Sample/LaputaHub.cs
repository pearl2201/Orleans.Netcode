using Netcode.Orleans.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Orleans.Websocket.Sample
{
    public class LaputaHub: WebSocketBehavior, IHub
    {
        private readonly ILogger<LaputaHub> _logger;
        private readonly IClusterClient _clusterClient;

        protected override void OnMessage(MessageEventArgs e)
        {
            var msg = e.Data == "BALUS"
                      ? "Are you kidding?"
                      : "I'm not available now.";

            Send(msg);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);

        }
    }
}
