using Unity.PerformanceTesting;

namespace UIComponents.Benchmarks
{
    public static class BenchmarkUtils
    {
        public const string Version = "0.9.0.1";
        
        private static SampleGroup[] GetProfilerMarkers()
        {
            return new[]
            {
                new SampleGroup("UIComponent.DependencySetup", SampleUnit.Microsecond),
                new SampleGroup("UIComponent.CacheSetup", SampleUnit.Microsecond),
                new SampleGroup("UIComponent.LayoutAndStylesSetup", SampleUnit.Microsecond),
                new SampleGroup("UIComponent.QueryFieldsSetup", SampleUnit.Microsecond)
            };
        }
        
        public static void MeasureComponentInitWithColdCache<TComponent>() where TComponent : UIComponent, new()
        {
            Measure.Method(() => { new TComponent(); })
                .SetUp(() =>
                {
                    UIComponent.ClearCache<TComponent>();
                    DependencyInjector.RemoveInjector(typeof(TComponent));
                })
                .SampleGroup(new SampleGroup("Cold Cache Time", SampleUnit.Microsecond))
                .ProfilerMarkers(GetProfilerMarkers())
                .MeasurementCount(50)
                .IterationsPerMeasurement(100)
                .GC()
                .Run();
        }

        public static void MeasureComponentInitWithWarmCache<TComponent>() where TComponent : UIComponent, new()
        {
            Measure.Method(() => { new TComponent(); })
                .SampleGroup(new SampleGroup("Warm Cache Time", SampleUnit.Microsecond))
                .MeasurementCount(50)
                .IterationsPerMeasurement(100)
                .ProfilerMarkers(GetProfilerMarkers())
                .GC()
                .Run();
        }
    }
}