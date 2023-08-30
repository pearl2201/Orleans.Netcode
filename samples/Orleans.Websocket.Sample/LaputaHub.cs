using Netcode.Orleans.Net;
using Orleans.Netcode;
using Orleans.Websocket.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Orleans.Websocket.Sample
{
    public class LaputaHub : WebsocketHub<LaputaHub>, IHub
    {

        public LaputaHub() : base() {
        
        }


        protected override void OnMessage(MessageEventArgs e)
        {
            //var msg = e.Data == "BALUS"
            //          ? "Are you kidding?"
            //          : "I'm not available now.";

            //Send(msg);
            HubLifetimeManager.SendAllAsync("echo", new object[] { e.Data });
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
