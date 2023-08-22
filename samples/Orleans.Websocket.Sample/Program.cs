using Netcode.Orleans.Net;
using Netcode.Orleans.Hosting;
using Orleans.Websocket.Sample;

IHost host = Host.CreateDefaultBuilder(args)
    .UseOrleans(siloBuilder =>
    {
        siloBuilder.UseLocalhostClustering();
        siloBuilder.UseSignalR(); // Adds ability #1 and #2 to Orleans.
        siloBuilder.RegisterHub<LaputaHub>(); // Required for each hub type if the backplane ability #1 is being used.
    })
    .ConfigureServices(services =>
    {



        services
        .AddWebsocket()
             // Adds SignalR hubs to the web application
            .AddOrleans();


    })
    .Build();
//app.MapHub<MyHub>("/myhub");
host.Run();
