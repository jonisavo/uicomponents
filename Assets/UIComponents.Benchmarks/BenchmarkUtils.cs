using Unity.PerformanceTesting;

namespace UIComponents.Benchmarks
{
    public static class BenchmarkUtils
    {
        public const string Version = "0.18.0.0";
        
        private static SampleGroup[] GetProfilerMarkers()
        {
            return new[]
            {
                new SampleGroup("UIComponent.DependencySetup"),
                new SampleGroup("UIComponent.CacheSetup"),
                new SampleGroup("UIComponent.LayoutAndStylesSetup"),
                new SampleGroup("UIComponent.PopulateFields")
            };
        }
        
        public static void MeasureComponentInitWithColdCache<TComponent>() where TComponent : UIComponent, new()
        {
            Measure.Method(() => { new TComponent(); })
                .SetUp(() =>
                {
                    UIComponent.ClearCache<TComponent>();
                    DependencyInjector.Container.Clear();
                })
                .SampleGroup(new SampleGroup("Cold Cache Time"))
                .ProfilerMarkers(GetProfilerMarkers())
                .MeasurementCount(50)
                .IterationsPerMeasurement(100)
                .GC()
                .Run();
        }

        public static void MeasureComponentInitWithWarmCache<TComponent>() where TComponent : UIComponent, new()
        {
            Measure.Method(() => { new TComponent(); })
                .SampleGroup(new SampleGroup("Warm Cache Time"))
                .MeasurementCount(50)
                .IterationsPerMeasurement(100)
                .ProfilerMarkers(GetProfilerMarkers())
                .GC()
                .Run();
        }
    }
}