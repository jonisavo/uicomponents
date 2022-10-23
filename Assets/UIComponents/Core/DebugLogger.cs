using UnityEngine;

namespace UIComponents
{
    /// <summary>
    /// The default logger for UIComponents. Uses Debug.Log.
    /// </summary>
    public class DebugLogger : ILogger
    {
        public void Log(string message, object caller)
        {
            Debug.LogFormat("[{0}] {1}", caller.GetType().Name, message);
        }
        
        public void LogWarning(string message, object caller)
        {
            Debug.LogWarningFormat("[{0}] {1}", caller.GetType().Name, message);
        }
        
        public void LogError(string message, object caller)
        {
            Debug.LogErrorFormat("[{0}] {1}", caller.GetType().Name, message);
        }
    }
}
