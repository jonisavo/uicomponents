using NUnit.Framework;
using Unity.PerformanceTesting;

namespace UIComponents.Benchmarks
{
    public class UIComponentInitBenchmarks
    {
        private class EmptyComponent : UIComponent {}
        
        private interface IDependency {}
        
        private class DependencyProvider : IDependency {}
        
        [Dependency(typeof(IDependency), provide: typeof(DependencyProvider))]
        private class ComponentWithDependency : UIComponent {}

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

