﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Netcode.Orleans.Net
{
    public class HubConnectionStore<THubConnectionContext> where THubConnectionContext: IHubConnectionContext
    {
        private readonly ConcurrentDictionary<string, THubConnectionContext> _connections =
       new ConcurrentDictionary<string, THubConnectionContext>(StringComparer.Ordinal);

        /// <summary>
        /// Get the <see cref="THubConnectionContext"/> by connection ID.
        /// </summary>
        /// <param name="connectionId">The ID of the connection.</param>
        /// <returns>The connection for the <paramref name="connectionId"/>, null if there is no connection.</returns>
        public THubConnectionContext? this[string connectionId]
        {
            get
            {
                _connections.TryGetValue(connectionId, out var connection);
                return connection;
            }
        }

        /// <summary>
        /// The number of connections in the store.
        /// </summary>
        public int Count => _connections.Count;

        /// <summary>
        /// Add a <see cref="THubConnectionContext"/> to the store.
        /// </summary>
        /// <param name="connection">The connection to add.</param>
        public void Add(THubConnectionContext connection)
        {
            _connections.TryAdd(connection.ConnectionId, connection);
        }

        /// <summary>
        /// Removes a <see cref="IHubConnectionContext"/> from the store.
        /// </summary>
        /// <param name="connection">The connection to remove.</param>
        public void Remove(THubConnectionContext connection)
        {
            _connections.TryRemove(connection.ConnectionId, out _);
        }

        /// <summary>
        /// Gets an enumerator over the connection store.
        /// </summary>
        /// <returns>The <see cref="Enumerator"/> over the connections.</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// An <see cref="IEnumerator"/> over the <see cref="HubConnectionStore"/>
        /// </summary>
        public readonly struct Enumerator : IEnumerator<THubConnectionContext>
        {
            private readonly IEnumerator<KeyValuePair<string, THubConnectionContext>> _enumerator;

            /// <summary>
            /// Constructs the <see cref="Enumerator"/> over the <see cref="HubConnectionStore"/>.
            /// </summary>
            /// <param name="hubConnectionList">The store of connections to enumerate over.</param>
            public Enumerator(HubConnectionStore<THubConnectionContext> hubConnectionList)
            {
                _enumerator = hubConnectionList._connections.GetEnumerator();
            }

            /// <summary>
            /// The current connection the enumerator is on.
            /// </summary>
            public THubConnectionContext Current => _enumerator.Current.Value;

            object IEnumerator.Current => Current;

            /// <summary>
            /// Disposes the enumerator.
            /// </summary>
            public void Dispose() => _enumerator.Dispose();

            /// <summary>
            /// Moves the enumerator to the next value.
            /// </summary>
            /// <returns>True if there is another connection. False if there are no more connections.</returns>
            public bool MoveNext() => _enumerator.MoveNext();

            /// <summary>
            /// Resets the enumerator to the beginning.
            /// </summary>
            public void Reset() => _enumerator.Reset();
        }
    }
}
