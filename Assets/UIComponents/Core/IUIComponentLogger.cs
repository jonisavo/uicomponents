namespace UIComponents
{
    public interface IUIComponentLogger
    {
        void Log(string message, UIComponent component);

        void LogWarning(string message, UIComponent component);
        
        void LogError(string message, UIComponent component);
    }
}