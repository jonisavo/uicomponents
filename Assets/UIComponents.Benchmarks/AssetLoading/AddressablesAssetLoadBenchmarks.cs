using System.Collections;
using NUnit.Framework;
using UIComponents.Addressables;
using Unity.PerformanceTesting;
using UnityEngine.TestTools;

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
        
        [UnitySetUp]
        public IEnumerator UnitySetUp()
        {
            var initOperation = UnityEngine.AddressableAssets.Addressables.InitializeAsync();

            while (!initOperation.IsDone)
                yield return null;
        }

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
