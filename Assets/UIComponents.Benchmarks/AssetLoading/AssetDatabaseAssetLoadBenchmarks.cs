using NUnit.Framework;
using UIComponents.Editor;
using Unity.PerformanceTesting;

namespace UIComponents.Benchmarks.AssetLoading
{
    public class AssetDatabaseAssetLoadBenchmarks
    {
        [Layout("AssetDatabaseLayoutFile.uxml")]
        [Stylesheet("AssetDatabaseStylesFile.uss")]
        [Stylesheet("CommonMargins.uss")]
        [AssetPath("Assets/UIComponents.Benchmarks/AssetLoading")]
        [Dependency(typeof(IAssetResolver), provide: typeof(AssetDatabaseAssetResolver))]
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