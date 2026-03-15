using UnityEngine.UIElements;
using UIComponents.DependencyInjection;

namespace UIComponents
{
    public interface IAssetResolver {}
    public class ResourcesAssetResolver : IAssetResolver {}

    public interface ILogger {}
    public class DebugLogger : ILogger {}

    [Dependency(typeof(IAssetResolver), typeof(ResourcesAssetResolver))]
    [Dependency(typeof(IAssetCatalog), typeof(DefaultAssetCatalog))]
    [Dependency(typeof(ILogger), typeof(DebugLogger))]
    public abstract class UIComponent : VisualElement, IDependencyConsumer
    {
        protected readonly ILogger? Logger;
        public readonly IAssetCatalog AssetCatalog;

        public class UxmlFactory<T> where T : UIComponent {}

        protected virtual void UIC_PopulateProvideFields() {}
    }
}
