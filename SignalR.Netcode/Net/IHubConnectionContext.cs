﻿using Orleans.Netcode.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Netcode.Orleans.Net
{
    public interface IHubConnectionContext
    {
        public string ConnectionId { get; set; }

        public CancellationToken ConnectionAborted { get; set; }

        public Task WriteAsync(InvocationMessage message);

        public bool IsAuthenticated { get; set; }

        public string UserIdentifier { get; set; }

    }
    //public static partial class Log
    //{
    //    [LoggerMessage(1, LogLevel.Debug, "Completed connection handshake. Using HubProtocol '{Protocol}'.", EventName = "HandshakeComplete")]
    //    public static partial void HandshakeComplete(ILogger logger, string protocol);

    //    [LoggerMessage(2, LogLevel.Debug, "Handshake was canceled.", EventName = "HandshakeCanceled")]
    //    public static partial void HandshakeCanceled(ILogger logger);

    //    [LoggerMessage(3, LogLevel.Trace, "Sent a ping message to the client.", EventName = "SentPing")]
    //    public static partial void SentPing(ILogger logger);

    //    [LoggerMessage(4, LogLevel.Debug, "Unable to send Ping message to client, the transport buffer is full.", EventName = "TransportBufferFull")]
    //    public static partial void TransportBufferFull(ILogger logger);

    //    [LoggerMessage(5, LogLevel.Debug, "Failed connection handshake.", EventName = "HandshakeFailed")]
    //    public static partial void HandshakeFailed(ILogger logger, Exception? exception);

    //    [LoggerMessage(6, LogLevel.Error, "Failed writing message. Aborting connection.", EventName = "FailedWritingMessage")]
    //    public static partial void FailedWritingMessage(ILogger logger, Exception exception);

    //    [LoggerMessage(7, LogLevel.Debug, "Server does not support version {Version} of the {Protocol} protocol.", EventName = "ProtocolVersionFailed")]
    //    public static partial void ProtocolVersionFailed(ILogger logger, string protocol, int version);

    //    [LoggerMessage(8, LogLevel.Trace, "Abort callback failed.", EventName = "AbortFailed")]
    //    public static partial void AbortFailed(ILogger logger, Exception exception);

    //    [LoggerMessage(9, LogLevel.Debug, "Client timeout ({ClientTimeout}ms) elapsed without receiving a message from the client. Closing connection.", EventName = "ClientTimeout")]
    //    public static partial void ClientTimeout(ILogger logger, TimeSpan clientTimeout);

    //    [LoggerMessage(10, LogLevel.Debug, "The maximum message size of {MaxMessageSize}B was exceeded while parsing the Handshake. The message size can be configured in AddHubOptions.", EventName = "HandshakeSizeLimitExceeded")]
    //    public static partial void HandshakeSizeLimitExceeded(ILogger logger, long maxMessageSize);
    //}
}
