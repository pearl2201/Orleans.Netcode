using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.Websocket.Config
{
    public class IWebsocketBuilder
    {
        public IServiceCollection Services { get; set; }
    }

    public class IWebsocketServerBuilder
    {
        public IServiceCollection Services { get; set; }
    }
}
