using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orleans.Netcode.Net
{
    public class InvocationMessage
    {
        /// <summary>
        /// Gets the target method name.
        /// </summary>
        public string Target { get; }

        /// <summary>
        /// Gets the target method arguments.
        /// </summary>
        public object?[] Arguments { get; }

        /// <summary>
        /// The target methods stream IDs.
        /// </summary>
        public string[]? StreamIds { get; }

        /// <summary>
        /// Gets the invocation ID.
        /// </summary>
        public string? InvocationId { get; }

        public IDictionary<string, string>? Headers { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubMethodInvocationMessage"/> class.
        /// </summary>
        /// <param name="invocationId">The invocation ID.</param>
        /// <param name="target">The target method name.</param>
        /// <param name="arguments">The target method arguments.</param>
        /// <param name="streamIds">The target methods stream IDs.</param>
        public InvocationMessage(string? invocationId, string target, object?[] arguments, string[]? streamIds)
            : this(invocationId, target, arguments)
        {
            StreamIds = streamIds;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HubMethodInvocationMessage"/> class.
        /// </summary>
        /// <param name="invocationId">The invocation ID.</param>
        /// <param name="target">The target method name.</param>
        /// <param name="arguments">The target method arguments.</param>
        public InvocationMessage(string? invocationId, string target, object?[] arguments)

        {
            this.InvocationId = invocationId;
            if (string.IsNullOrEmpty(target))
            {
                throw new ArgumentNullException(nameof(target));
            }

            Target = target;
            Arguments = arguments;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvocationMessage"/> class.
        /// </summary>
        /// <param name="target">The target method name.</param>
        /// <param name="arguments">The target method arguments.</param>
        public InvocationMessage(string target, object?[] arguments)
            : this(null, target, arguments)
        {
        }





        /// <inheritdoc />
        public override string ToString()
        {
            string args;
            string streamIds;
            try
            {
                args = Arguments == null ? string.Empty : string.Join(", ", Arguments.Select(a => a?.ToString()));
            }
            catch (Exception ex)
            {
                args = $"Error: {ex.Message}";
            }

            try
            {
                streamIds = string.Join(", ", StreamIds != null ? StreamIds.Select(id => id?.ToString()) : Array.Empty<string>());
            }
            catch (Exception ex)
            {
                streamIds = $"Error: {ex.Message}";
            }

            return $"InvocationMessage {{ {nameof(InvocationId)}: \"{InvocationId}\", {nameof(Target)}: \"{Target}\", {nameof(Arguments)}: [ {args} ], {nameof(StreamIds)}: [ {streamIds} ] }}";
        }
    }
}
