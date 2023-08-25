using System;
using Microsoft.Extensions.Logging;
using Netcode.Orleans;
using Netcode.Orleans.Core;
using Netcode.Orleans.Net;
using Orleans.Runtime;
using Orleans.Streams;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Orleans.Netcode.Net;

namespace Orleans.Netcode
{
    public sealed class OrleansHubLifetimeManager<THub, THubConnectionContext> : IHubLifetimeManager, ILifecycleParticipant<ISiloLifecycle>,
       IDisposable where THub : IHub where THubConnectionContext : HubConnectionContext
    {
        private readonly Guid _serverId;
        private readonly ILogger _logger;
        private readonly string _hubName;
        private readonly IClusterClient _clusterClient;
        private readonly SemaphoreSlim _streamSetupLock = new(1);
        private readonly HubConnectionStore<THubConnectionContext> _connections = new();

        private IStreamProvider? _streamProvider;
        private IAsyncStream<ClientMessage> _serverStream = default!;
        private IAsyncStream<AllMessage> _allStream = default!;
        private Timer _timer = default!;

        public OrleansHubLifetimeManager(
            ILogger<OrleansHubLifetimeManagerv1<THub, THubConnectionContext>> logger,
            IClusterClient clusterClient
        )
        {
            var hubType = typeof(THub).BaseType?.GenericTypeArguments.FirstOrDefault() ?? typeof(THub);
            _hubName = hubType.IsInterface && hubType.Name[0] == 'I'
                ? hubType.Name[1..]
                : hubType.Name;
            _serverId = Guid.NewGuid();
            _logger = logger;
            _clusterClient = clusterClient;
        }

        private Task HeartbeatCheck()
          => _clusterClient.GetServerDirectoryGrain().Heartbeat(_serverId);

        private async Task EnsureStreamSetup()
        {
            if (_streamProvider is not null)
                return;

            await _streamSetupLock.WaitAsync();

            try
            {
                if (_streamProvider is not null)
                    return;

                _logger.LogInformation(
                    "Initializing: Orleans HubLifetimeManager {hubName} (serverId: {serverId})...",
                    _hubName, _serverId);

                _streamProvider = _clusterClient.GetOrleansSignalRStreamProvider();
                _serverStream = _streamProvider.GetServerStream(_serverId);
                _allStream = _streamProvider.GetAllStream(_hubName);

                _timer = new Timer(
                    _ => Task.Run(HeartbeatCheck), null, TimeSpan.FromSeconds(0),
                    TimeSpan.FromMinutes(SignalROrleansConstants.SERVER_HEARTBEAT_PULSE_IN_MINUTES));

                await Task.WhenAll(
                    _allStream.SubscribeAsync((msg, _) => ProcessAllMessage(msg)),
                    _serverStream.SubscribeAsync((msg, _) => ProcessServerMessage(msg))
                );

                _logger.LogInformation(
                    "Initialized complete: Orleans HubLifetimeManager {hubName} (serverId: {serverId})",
                    _hubName, _serverId);
            }
            finally
            {
                _streamSetupLock.Release();
            }
        }

        private Task ProcessAllMessage(AllMessage allMessage)
        {
            var allTasks = new List<Task>(_connections.Count);
            var payload = allMessage.Message!;

            foreach (var connection in _connections)
            {
                if (connection.ConnectionAborted.IsCancellationRequested)
                    continue;

                if (allMessage.ExcludedIds == null || !allMessage.ExcludedIds.Contains(connection.ConnectionId))
                    allTasks.Add(SendLocal(connection, payload));
            }

            return Task.WhenAll(allTasks);
        }

        private Task ProcessServerMessage(ClientMessage clientMessage)
        {
            var connection = _connections[clientMessage.ConnectionId];
            return connection == null ? Task.CompletedTask : SendLocal(connection, clientMessage.Message);
        }

        public async Task OnConnectedAsync(THubConnectionContext connection)
        {
            await EnsureStreamSetup();

            try
            {
                _connections.Add(connection);

                var client = _clusterClient.GetClientGrain(_hubName, connection.ConnectionId);
                await client.OnConnect(_serverId);

                if (connection!.IsAuthenticated)
                {
                    var user = _clusterClient.GetUserGrain(_hubName, connection.UserIdentifier!);
                    await user.Add(connection.ConnectionId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "An error has occurred 'OnConnectedAsync' while adding connection {connectionId} [hub: {hubName} (serverId: {serverId})]",
                    connection?.ConnectionId, _hubName, _serverId);
                _connections.Remove(connection!);
                throw;
            }
        }

        private Task SendLocal(HubConnectionContext connection, InvocationMessage hubMessage)
        {
            _logger.LogDebug(
                "Sending local message to connection {connectionId} on hub {hubName} (serverId: {serverId})",
                connection.ConnectionId, _hubName, _serverId);
            return connection.WriteAsync(hubMessage);
        }

  

        public async Task OnDisconnectedAsync(THubConnectionContext connection)
        {
            try
            {
                _logger.LogDebug("Handle disconnection {connectionId} on hub {hubName} (serverId: {serverId})",
                    connection.ConnectionId, _hubName, _serverId);
                var client = _clusterClient.GetClientGrain(_hubName, connection.ConnectionId);
                await client.OnDisconnect("hub-disconnect");
            }
            finally
            {
                _connections.Remove(connection);
            }
        }

        public void Participate(ISiloLifecycle lifecycle)
        {
            lifecycle.Subscribe(
                   observerName: nameof(OrleansHubLifetimeManagerv1<THub, THubConnectionContext>),
                   stage: ServiceLifecycleStage.Active,
                   onStart: async cts => await Task.Run(EnsureStreamSetup, cts));
        }

        public void Dispose()
        {
            _timer?.Dispose();

            var toUnsubscribe = new List<Task>();
            if (_serverStream is not null)
            {
                toUnsubscribe.Add(Task.Factory.StartNew(async () =>
                {
                    var subscriptions = await _serverStream.GetAllSubscriptionHandles();
                    var subs = new List<Task>();
                    subs.AddRange(subscriptions.Select(s => s.UnsubscribeAsync()));
                    await Task.WhenAll(subs);
                }));
            }

            if (_allStream is not null)
            {
                toUnsubscribe.Add(Task.Factory.StartNew(async () =>
                {
                    var subscriptions = await _allStream.GetAllSubscriptionHandles();
                    var subs = new List<Task>();
                    subs.AddRange(subscriptions.Select(s => s.UnsubscribeAsync()));
                    await Task.WhenAll(subs);
                }));
            }

            var serverDirectoryGrain = _clusterClient.GetServerDirectoryGrain();
            toUnsubscribe.Add(serverDirectoryGrain.Unregister(_serverId));

            Task.WhenAll(toUnsubscribe.ToArray()).GetAwaiter().GetResult();
        }

        public Task SendAllAsync(string methodName, object?[] args,
  CancellationToken cancellationToken = default)
        {
            var message = new InvocationMessage(methodName, args);
            return _allStream.OnNextAsync(new AllMessage(message));
        }

        public Task SendConnectionAsync(string connectionId, string methodName, object?[] args,
    CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(connectionId)) throw new ArgumentNullException(nameof(connectionId));
            if (string.IsNullOrWhiteSpace(methodName)) throw new ArgumentNullException(nameof(methodName));

            var message = new InvocationMessage(methodName, args);

            var connection = _connections[connectionId];
            if (connection != null) return SendLocal(connection, message);

            return SendExternal(connectionId, message);
        }


        private Task SendExternal(string connectionId, InvocationMessage hubMessage)
        {
            var client = _clusterClient.GetClientGrain(_hubName, connectionId);
            return client.Send(hubMessage);
        }

        public  Task SendGroupAsync(string groupName, string methodName, object?[] args,
    CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(groupName)) throw new ArgumentNullException(nameof(groupName));
            if (string.IsNullOrWhiteSpace(methodName)) throw new ArgumentNullException(nameof(methodName));

            var group = _clusterClient.GetGroupGrain(_hubName, groupName);
            return group.Send(methodName, args);
        }

        public Task SendUserAsync(string userId, string methodName, object?[] args,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException(nameof(userId));
            if (string.IsNullOrWhiteSpace(methodName)) throw new ArgumentNullException(nameof(methodName));

            var user = _clusterClient.GetUserGrain(_hubName, userId);
            return user.Send(methodName, args);
        }

        public Task SendUsersAsync(IReadOnlyList<string> userIds, string methodName, object?[] args,
            CancellationToken cancellationToken = default)
        {
            var tasks = userIds.Select(u => SendGroupAsync(u, methodName, args, cancellationToken));
            return Task.WhenAll(tasks);
        }

        public Task AddToGroupAsync(string connectionId, string groupName,
            CancellationToken cancellationToken = default)
        {
            var group = _clusterClient.GetGroupGrain(_hubName, groupName);
            return group.Add(connectionId);
        }

        public Task RemoveFromGroupAsync(string connectionId, string groupName,
            CancellationToken cancellationToken = default)
        {
            var group = _clusterClient.GetGroupGrain(_hubName, groupName);
            return group.Remove(connectionId);
        }
    }
}

