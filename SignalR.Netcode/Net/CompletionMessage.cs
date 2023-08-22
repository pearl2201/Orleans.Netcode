namespace Orleans.Netcode.Net
{
    public class CompletionMessage
    {
        /// <summary>
        /// Optional error message if the invocation wasn't completed successfully. This must be null if there is a result.
        /// </summary>
        public string? Error { get; }

        /// <summary>
        /// Optional result from the invocation. This must be null if there is an error.
        /// This can also be null if there wasn't a result from the method invocation.
        /// </summary>
        public object? Result { get; }

        /// <summary>
        /// Specifies whether the completion contains a result.
        /// </summary>
        public bool HasResult { get; }
    }
}
