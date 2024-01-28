using NUnit.Framework;
using UIComponents.Addressables;
using Unity.PerformanceTesting;

namespace UIComponents.Benchmarks.AssetLoading
{
    [TestFixture]
    public partial class AddressablesAssetLoadBenchmarks
    {
        [AssetPrefix("Assets/Samples/Addressables/Data/")]
        [Layout("AddressablesExampleComponent.uxml")]
        [Stylesheet("AddressablesExampleComponent.uss")]
        [Stylesheet("Box.uss")]
        [Dependency(typeof(IAssetResolver), provide: typeof(AddressableAssetResolver), Scope.Transient)]
        private partial class ComponentWithAssets : UIComponent {}

        // NOTE(joni): This test fails on Unity 2023.2 in CI, but not locally.
        // The benchmark logic is questionable anyway since it uses an async lambda,
        // so it may be necessary to rework the benchmarks.
        // [Test, Performance, Version(BenchmarkUtils.Version)]
        // public void InitializeComponentWithWarmCache()
        // {
        //     BenchmarkUtils.MeasureComponentInitWithWarmCache<ComponentWithAssets>();
        // }
        
        [Test, Performance, Version(BenchmarkUtils.Version)]
        public void InitializeComponentWithColdCache()
        {
            BenchmarkUtils.MeasureComponentInitWithColdCache<ComponentWithAssets>();
        }
    }
}
