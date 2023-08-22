using Microsoft.Extensions.DependencyInjection;
using Netcode.Orleans.Config;
using Netcode.Orleans.Net;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Netcode.Orleans.Hosting
{
    public static class ISiloBuilderExtensions
    {
        public static ISiloBuilder UseSignalR(this ISiloBuilder builder, Action<NetOrleansSiloHostConfigBuilder>? configure = null)
        {
            var cfg = new NetOrleansSiloHostConfigBuilder();
            configure?.Invoke(cfg);

            cfg.ConfigureBuilder?.Invoke(builder);

            try
            {
                builder.AddMemoryGrainStorage(SignalROrleansConstants.PUBSUB_STORAGE_PROVIDER); // "ORLEANS_SIGNALR_PUBSUB_PROVIDER"
            }
            catch
            {
                /** PubSubStore was already added. Do nothing. **/
            }

            try
            {
                builder.AddMemoryGrainStorage(SignalROrleansConstants.SIGNALR_ORLEANS_STORAGE_PROVIDER); // "ORLEANS_SIGNALR_STORAGE_PROVIDER"
            }
            catch
            {
                /** Grain storage provider was already added. Do nothing. **/
            }

            builder.AddMemoryStreams(SignalROrleansConstants.SIGNALR_ORLEANS_STREAM_PROVIDER); // "ORLEANS_SIGNALR_STREAM_PROVIDER"

            return builder;
        }

        public static ISiloBuilder RegisterHub<THub>(this ISiloBuilder builder) where THub : IHub
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient<ILifecycleParticipant<ISiloLifecycle>>(sp =>
                    (sp.GetRequiredService<HubLifetimeManager<THub>>() as ILifecycleParticipant<ISiloLifecycle>)!);
            });

            return builder;
        }
    }
}
