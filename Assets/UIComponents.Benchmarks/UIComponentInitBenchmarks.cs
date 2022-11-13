using NUnit.Framework;
using Unity.PerformanceTesting;

namespace UIComponents.Benchmarks
{
    public partial class UIComponentInitBenchmarks
    {
        private partial class EmptyComponent : UIComponent {}
        
        private interface IMockDependency {}
        
        private class DependencyProvider : IMockDependency {}
        
        [Dependency(typeof(IMockDependency), provide: typeof(DependencyProvider))]
        private partial class ComponentWithDependency : UIComponent {}

        [Test, Performance, Version(BenchmarkUtils.Version)]
        public void InitializeEmptyComponentWithWarmCache()
        {
            BenchmarkUtils.MeasureComponentInitWithWarmCache<EmptyComponent>();
        }
        
        [Test, Performance, Version(BenchmarkUtils.Version)]
        public void InitializeEmptyComponentWithColdCache()
        {
            BenchmarkUtils.MeasureComponentInitWithColdCache<EmptyComponent>();
        }
        
        [Test, Performance, Version(BenchmarkUtils.Version)]
        public void InitializeComponentWithDependencyWithWarmCache()
        {
            BenchmarkUtils.MeasureComponentInitWithWarmCache<ComponentWithDependency>();
        }
        
        [Test, Performance, Version(BenchmarkUtils.Version)]
        public void InitializeComponentWithDependencyWithColdCache()
        {
            BenchmarkUtils.MeasureComponentInitWithColdCache<ComponentWithDependency>();
        }
    }
}

