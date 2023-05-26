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
        [Dependency(typeof(IAssetResolver), provide: typeof(AssetDatabaseAssetResolver), Scope.Transient)]
        private partial class ComponentWithAssets : UIComponent {}

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
