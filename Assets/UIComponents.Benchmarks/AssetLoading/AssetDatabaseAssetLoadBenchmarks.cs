using NUnit.Framework;
using UIComponents.Editor;
using Unity.PerformanceTesting;

namespace UIComponents.Benchmarks.AssetLoading
{
    [TestFixture]
    public partial class AssetDatabaseAssetLoadBenchmarks
    {
        [AssetPrefix("Assets/Samples/Addressables/Data/")]
        [Layout("AddressablesExampleComponent.uxml")]
        [Stylesheet("AddressablesExampleComponent.uss")]
        [Stylesheet("Box.uss")]
        [Dependency(typeof(IAssetSource), provide: typeof(AssetDatabaseAssetSource), Scope.Transient)]
        private partial class ComponentWithAssets : UIComponent {}

        [Test, Performance, Version(BenchmarkUtils.Version), Timeout(5000)]
        public void InitializeComponentWithWarmCache()
        {
            BenchmarkUtils.MeasureComponentInitWithWarmCache<ComponentWithAssets>();
        }
        
        [Test, Performance, Version(BenchmarkUtils.Version), Timeout(5000)]
        public void InitializeComponentWithColdCache()
        {
            BenchmarkUtils.MeasureComponentInitWithColdCache<ComponentWithAssets>();
        }
    }
}
