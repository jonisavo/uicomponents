using NUnit.Framework;
using UIComponents.Addressables;
using Unity.PerformanceTesting;

namespace UIComponents.Benchmarks.AssetLoading
{
    [TestFixture]
    public class AddressablesAssetLoadBenchmarks
    {
        [AssetPath("Assets/Samples/Addressables/Data")]
        [Layout("AddressablesExampleComponent.uxml")]
        [Stylesheet("AddressablesExampleComponent.uss")]
        [Stylesheet("Box.uss")]
        [Dependency(typeof(IAssetResolver), provide: typeof(AddressableAssetResolver), Scope.Transient)]
        private class ComponentWithAssets : UIComponent {}

        [Test, Performance, Version(BenchmarkUtils.Version)]
        public void InitializeComponentWithWarmCache()
        {
            BenchmarkUtils.MeasureComponentInitWithWarmCache<ComponentWithAssets>();
        }
        
        [Test, Performance, Version(BenchmarkUtils.Version)]
        public void InitializeComponentWithColdCache()
        {
            BenchmarkUtils.MeasureComponentInitWithColdCache<ComponentWithAssets>();
        }
    }
}
