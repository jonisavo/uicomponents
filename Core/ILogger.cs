namespace UIComponents
{
    /// <summary>
    /// An interface for logging information in UIComponents.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a regular message.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="caller">Object which logged this message</param>
        void Log(string message, object caller);

        /// <summary>
        /// Logs a warning.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="caller">Object which logged this message</param>
        void LogWarning(string message, object caller);
        
        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="caller">Object which logged this message</param>
        void LogError(string message, object caller);
    }
}
