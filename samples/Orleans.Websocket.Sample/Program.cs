using Netcode.Orleans.Net;
using Netcode.Orleans.Hosting;
using Orleans.Websocket.Sample;
using Orleans.Websocket;
using Orleans.Websocket.Net;
using Netcode.Orleans;
using Orleans.Hosting;

var hostBuilder = Host.CreateDefaultBuilder(args);
if (int.Parse(args[0]) == 0)
{
    hostBuilder.UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering()
        .UseSignalR();

        siloBuilder.RegisterHub<LaputaHub, WebsocketHubConnectionContext>(); // Required for each hub type if the backplane ability #1 is being used.
    });
}
else
{
    hostBuilder.UseOrleansClient(clientBuilder =>
    {
        clientBuilder.UseLocalhostClustering()
       .UseWebsocket((a) =>
       {

       });
    });
}


hostBuilder
.ConfigureServices(services =>
{



    services
    .AddWebsocket((cfg) =>
    {
        cfg.Port = int.Parse(args[1]);
        cfg.AddHub<LaputaHub>("/laputa", (host) =>
        {

        });

    })
        // Adds SignalR hubs to the web application
        .AddOrleans<LaputaHub>();


});

IHost host = hostBuilder.Build();
//app.MapHub<MyHub>("/myhub");
host.Run();
