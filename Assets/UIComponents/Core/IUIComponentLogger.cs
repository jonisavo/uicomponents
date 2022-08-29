namespace UIComponents
{
    /// <summary>
    /// An interface for logging information in UIComponents.
    /// </summary>
    public interface IUIComponentLogger
    {
        /// <summary>
        /// Logs a regular message.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="component">UIComponent which logged this message</param>
        void Log(string message, UIComponent component);

        /// <summary>
        /// Logs a warning.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="component">UIComponent which logged this message</param>
        void LogWarning(string message, UIComponent component);
        
        /// <summary>
        /// Logs an error.
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="component">UIComponent which logged this message</param>
        void LogError(string message, UIComponent component);
    }
}
