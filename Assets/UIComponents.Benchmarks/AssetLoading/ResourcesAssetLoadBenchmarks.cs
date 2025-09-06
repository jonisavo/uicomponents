using NUnit.Framework;
using Unity.PerformanceTesting;

namespace UIComponents.Benchmarks.AssetLoading
{
    public partial class ResourcesAssetLoadBenchmarks
    {
        [Layout("Components/ResourcesExampleComponent")]
        [Stylesheet("Components/ResourcesExampleComponent.style")]
        [Stylesheet("Margins")]
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
