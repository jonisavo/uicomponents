using UnityEngine;

namespace UIComponents
{
    public class UIComponentDebugLogger : IUIComponentLogger
    {
        public void Log(string message, UIComponent component)
        {
            Debug.LogFormat("[{0}] {1}", component.GetTypeName(), message);
        }
        
        public void LogWarning(string message, UIComponent component)
        {
            Debug.LogWarningFormat("[{0}] {1}", component.GetTypeName(), message);
        }
        
        public void LogError(string message, UIComponent component)
        {
            Debug.LogErrorFormat("[{0}] {1}", component.GetTypeName(), message);
        }
    }
}