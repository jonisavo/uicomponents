using UnityEngine.UIElements;
using UIComponents.DependencyInjection;

namespace UIComponents
{
    public interface IAssetResolver {}
    public class ResourcesAssetResolver : IAssetResolver {}

    public interface ILogger {}
    public class DebugLogger : ILogger {}

    [Dependency(typeof(IAssetResolver), typeof(ResourcesAssetResolver))]
    [Dependency(typeof(ILogger), typeof(DebugLogger))]
    public abstract class UIComponent : VisualElement, IDependencyConsumer
    {
        public class UxmlFactory<T> where T : UIComponent {}
    }
}
