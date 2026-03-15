using UnityEngine.UIElements;
using UIComponents.DependencyInjection;

namespace UIComponents
{
    public interface IAssetSource {}
    public class ResourcesAssetSource : IAssetSource {}

    public interface ILogger {}
    public class DebugLogger : ILogger {}

    [Dependency(typeof(IAssetSource), typeof(ResourcesAssetSource))]
    [Dependency(typeof(ILogger), typeof(DebugLogger))]
    public abstract class UIComponent : VisualElement, IDependencyConsumer
    {
        protected readonly ILogger? Logger;
        public readonly IAssetSource AssetSource;

        public class UxmlFactory<T> where T : UIComponent {}

        protected virtual void UIC_PopulateProvideFields() {}
    }
}
